using System;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Web.Security;
using HowToGet.BusinessLogic.Configuration;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Hosting.Interfaces;
using HowToGet.Models.Analytics;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Web.Filters;
using HowToGet.Web.Helpers;
using HowToGet.Web.Models;
using NLog;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	public class OAuthController : ApiController
    {
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

		private readonly IUserProvider _userProvider;
		private readonly IUploadToHostingProvider _uploadProvider;
		private readonly IActionEvents _actionEvents;
		//private readonly IInviteProvider _inviteProvider;

	    public OAuthController(IUserProvider userProvider, IUploadToHostingProvider uploadProvider, IActionEvents actionEvents)
	    {
		    _userProvider = userProvider;
		    _uploadProvider = uploadProvider;
		    _actionEvents = actionEvents;
		    //_inviteProvider = inviteProvider;
	    }

	    // POST: /auth/extLogin
		//[Https]
		[HttpPost]
		public AuthModel ExternalLogin(ExternalLoginModel loginModel)
		{
			var userId = _userProvider.GetExternalUserId(loginModel.Id, loginModel.AuthService);
			MembershipUserEx user;
			if (!string.IsNullOrEmpty(userId))
			{
				user = Membership.GetUser((object)userId, true) as MembershipUserEx;
				if (user != null)
					return new AuthModel(user);
			}

			if (string.IsNullOrWhiteSpace(loginModel.Email))
				throw new SecurityException("EmailIsEmpty");

			var gender = string.IsNullOrEmpty(loginModel.Gender)
							 ? GenderTypes.NotSet
							 : loginModel.Gender == "male"
								   ? GenderTypes.Male
								   : GenderTypes.Female;

			user = Membership.GetUser(loginModel.Email, true) as MembershipUserEx;

			bool isSmthChanged = false;
			bool isNewUser = false;
			string password = string.Empty;

			if (user == null)
			{
				//if (string.IsNullOrEmpty(loginModel.InviteCode))
				//	throw new SecurityException("InviteIsEmpty");

				//string inviteId;
				//if (!_inviteProvider.CheckInviteCode(loginModel.InviteCode, out inviteId))
				//	throw new SecurityException("UnknownInviteCode");

				string name = string.Format("{0} {1}", loginModel.FirstName, loginModel.LastName);
				password = _userProvider.GeneratePassword();

				user = Membership.CreateUser(name, password, loginModel.Email) as MembershipUserEx;
				//_inviteProvider.UseInvite(inviteId, user.ProviderUserKey.ToString());
				//_inviteProvider.GenerateInvites(user.ProviderUserKey.ToString());

				FetchUserPicture(loginModel.Picture, user.ProviderUserKey.ToString());

				user.Gender = gender;
				user.HomeCity = loginModel.City;
				user.HomeCountry = loginModel.Country;
				isSmthChanged = true;
				isNewUser = true;
			}
			else
			{
				if (user.Gender != gender)
				{
					user.Gender = gender;
					isSmthChanged = true;
				}
				FetchUserPicture(loginModel.Picture, user.ProviderUserKey.ToString());
				if (string.IsNullOrEmpty(user.HomeCityId))
				{
					user.HomeCity = loginModel.City;
					isSmthChanged = true;
				}
				if (string.IsNullOrEmpty(user.HomeCountryId))
				{
					user.HomeCountry = loginModel.Country;
					isSmthChanged = true;
				}
			}
			_userProvider.AssociateExternalUser(user, loginModel.Id, loginModel.AuthService, loginModel.AccessToken);
			if (isSmthChanged)
			{
				if (!string.IsNullOrEmpty(user.HomeCity) || !string.IsNullOrEmpty(user.HomeCountry))
				{
					_userProvider.FillAddressData(user);
				}
				Membership.UpdateUser(user);
			}
			if (isNewUser)
			{
				_userProvider.ProcessExternalUserCreated(user, password, loginModel.AuthService);

				var registerAction = new RegisterAction(user.ProviderUserKey.ToString(), loginModel.Referrer, ClientHelper.GetClientIp(Request));
				_actionEvents.OnUserAction(registerAction);
			}
			return new AuthModel(user);
		}

		private void FetchUserPicture(string picture, string userId)
		{
			if (string.IsNullOrEmpty(picture))
				return;

			var uri = new Uri(picture);

			var fileName = Path.Combine(ImagesConfig.Instance.ImagePath, UserPicFileNameGenerator.GenerateFileName(userId, Path.GetExtension(picture)));

			var client = new WebClient();
			client.DownloadFileCompleted += (sender, e) =>
			{
				if (e.Cancelled || e.Error != null)
				{
					CurrentLogger.ErrorException(string.Format("Failed to load user picture for user {0} from URL {1}", userId, picture), e.Error);
				}
				else
				{
					var uploadedFileName = _uploadProvider.PrepareAndUploadImage(fileName);
					_userProvider.UpdateUserPicture(userId, uploadedFileName);
				}
			};

			client.DownloadFileAsync(uri, fileName);
		}
    }
}
