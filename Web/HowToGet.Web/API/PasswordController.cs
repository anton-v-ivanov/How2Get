using System;
using System.Web.Http;
using System.Web.Security;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Web.Filters;
using HowToGet.Web.Models;
using HowToGet.Web.Helpers;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	public class PasswordController : ApiController
    {
		private readonly IUserProvider _userProvider;

		public PasswordController(IUserProvider userProvider)
		{
			_userProvider = userProvider;
		}

		[HttpPut]
		[Authorize]
		public AuthModel ChangePassword(ChangePasswordModel changePasswordModel)
		{
			var userId = User.Identity.Name;
			var user = Membership.GetUser((object)userId, true) as MembershipUserEx;
			if (user == null)
				throw new InternalErrorException("UserNotFound");

			try
			{
				if (!user.ChangePassword(changePasswordModel.OldPassword, changePasswordModel.NewPassword))
					throw new ValidationException("OldPasswordWrong");
				return new AuthModel(user);
			}
			catch (ValidationException)
			{
				throw;
			}
			catch (ArgumentException exception)
			{
				throw new ValidationException(exception.Message);
			}
			catch (MembershipPasswordException exception)
			{
				throw new ValidationException(exception.Message);
			}
			catch (Exception exception)
			{
				throw new InternalErrorException(exception.Message);
			}
		}

		[HttpPost]
		public void ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			if(forgotPasswordModel == null || string.IsNullOrEmpty(forgotPasswordModel.Email))
				throw new ValidationException("EmailIsEmpty");

			var users = Membership.FindUsersByEmail(forgotPasswordModel.Email);
			if (users.Count != 1)
				throw new SecurityException("EmailNotFound");

			_userProvider.ForgotPassword(users.First());
		}
    }
}
