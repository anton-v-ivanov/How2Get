using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Notifications;
using HowToGet.Models.Users;
using HowToGet.Notifications.Interfaces;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;

namespace HowToGet.BusinessLogic.Providers
{
	public class UserProvider : IUserProvider
	{
		#region Repositories

		private readonly IUserRepository _userRepository;
		private readonly IRouteRepository _routeRepository;
		private readonly IEmailNotificationProvider _notificationProvider;
		private readonly IOneTimeTokenProvider _oneTimeTokenProvider;

		#endregion

		#region Providers

		private readonly ICountryProvider _countryProvider;
		private readonly ICityProvider _cityProvider;
		
		#endregion

		public UserProvider(IUserRepository userRepository, IRouteRepository routeRepository, ICountryProvider countryProvider, ICityProvider cityProvider, 
							IEmailNotificationProvider notificationProvider, IOneTimeTokenProvider oneTimeTokenProvider)
		{
			_userRepository = userRepository;
			_routeRepository = routeRepository;
			_countryProvider = countryProvider;
			_cityProvider = cityProvider;
			_notificationProvider = notificationProvider;
			_oneTimeTokenProvider = oneTimeTokenProvider;
		}
		
		public void ForgotPassword(MembershipUserEx user)
		{
			var token = _oneTimeTokenProvider.Generate(user.ProviderUserKey.ToString());
			_notificationProvider.Send(EmailNotificationType.ForgotPassword, user.ProviderUserKey.ToString(), new Dictionary<string, object> { { "token", token } });
		}

		public string GetExternalUserId(string externalId, ExternalAuthServices authService)
		{
			return _userRepository.GetUserIdByExternalId(externalId, authService);
		}

		public string GeneratePassword()
		{
			return Membership.GeneratePassword(8, 2);
		}

		public void AssociateExternalUser(MembershipUserEx user, string externalId, ExternalAuthServices authService, string accessToken)
		{
			var objectId = MongoHelper.GetObjectIdFromString(user.ProviderUserKey.ToString(), string.Format("{0} is not valid user id", user.ProviderUserKey));
			_userRepository.AssociateExternalUser(new ExternalUserLink(objectId, externalId, authService, accessToken));
		}

		public void UpdateUserPicture(string userId, string fileName)
		{
			var objectId = MongoHelper.GetObjectIdFromString(userId, string.Format("{0} is not valid user id", userId));
			_userRepository.UpdateUserPicture(objectId, fileName);
		}

		public User GetUserById(string userId)
		{
			var objectId = MongoHelper.GetObjectIdFromString(userId, string.Format("{0} is not valid user id", userId));
			return _userRepository.GetUser(objectId, false);
		}

		public void SendUserCreatedNotification(MembershipUserEx user)
		{
			_notificationProvider.Send(EmailNotificationType.Registered, user.ProviderUserKey.ToString(), null);
		}

		public void SendUserCreatedWithPasswordNotification(MembershipUserEx user, string password)
		{
			_notificationProvider.Send(EmailNotificationType.RegisteredWithAutoPassword, user.ProviderUserKey.ToString(), new Dictionary<string, object> { { "password", password } });
		}

		public void ProcessExternalUserCreated(MembershipUserEx user, string password, ExternalAuthServices service)
		{
			var serviceStr = string.Empty;
			switch (service)
			{
				case ExternalAuthServices.Facebook:
					serviceStr = "Facebook";
					break;
				case ExternalAuthServices.Google:
					serviceStr = "Google";
					break;
				case ExternalAuthServices.Vk:
					serviceStr = "VKontakte";
					break;
				case ExternalAuthServices.FourSquare:
					serviceStr = "FourSquare";
					break;
			}

			_notificationProvider.Send(EmailNotificationType.RegisteredExternal, user.ProviderUserKey.ToString(), new Dictionary<string, object> { { "password", password }, { "service", serviceStr } });
		}

		public IEnumerable<MembershipUserEx> GetTopUsers()
		{
			var userIds = _routeRepository.GetTopUsers();
			var result = new List<MembershipUserEx>();
			foreach (var userId in userIds)
			{
				var objectId = MongoHelper.GetObjectIdFromString(userId, string.Format("{0} is not valid user id", userId));
				var user = Membership.GetUser(objectId, false) as MembershipUserEx;
				FillAddressData(user);
				result.Add(user);
			}
			return result;
		}

		public void FillAddressData(MembershipUserEx user)
		{
			if (!string.IsNullOrEmpty(user.HomeCountryId))
				user.HomeCountry = _countryProvider.GetCountryById(user.HomeCountryId).Name;

			if (!string.IsNullOrEmpty(user.HomeCityId))
				user.HomeCity = _cityProvider.GetById(user.HomeCityId).Name;

			// айдишники не заполнены - поиск по имени
			if (string.IsNullOrEmpty(user.HomeCityId) && string.IsNullOrEmpty(user.HomeCountryId))
			{
				// заполнен город
				if (!string.IsNullOrEmpty(user.HomeCity))
				{
					var cities = _cityProvider.SearchCity(user.HomeCity);
					// если есть точное совпадение - подставляем его
					if (cities.Count == 1)
					{
						user.HomeCityId = cities[0].Id;
						user.HomeCountryId = cities[0].CountryId;
					}
					// если нет точного совпадения, и заполнена страна - ищем в найденных город в этой стране
					else if (cities.Count > 0)
					{
						switch (user.HomeCountry.ToLowerInvariant())
						{
							case "россия":
								user.HomeCountry = "Russia";
								break;
							case "украина":
								user.HomeCountry = "Ukraine";
								break;
							case "казахстан":
								user.HomeCountry = "Kazakhstan";
								break;
							case "беларусь":
								user.HomeCountry = "Belarus";
								break;
							case "азербайджан":
								user.HomeCountry = "Azerbaijan";
								break;
							case "армения":
								user.HomeCountry = "Armenia";
								break;
							case "израиль":
								user.HomeCountry = "Israel";
								break;
							case "сша":
								user.HomeCountry = "United States";
								break;
							case "германия":
								user.HomeCountry = "Germany";
								break;
							case "кыргыстан":
								user.HomeCountry = "Kyrgyzstan";
								break;
							case "латвия":
								user.HomeCountry = "Latvia";
								break;
							case "литва":
								user.HomeCountry = "Lithuania";
								break;
							case "эстония":
								user.HomeCountry = "Estonia";
								break;
							case "молдова":
								user.HomeCountry = "Moldova";
								break;
							case "таджикистан":
								user.HomeCountry = "Tajikistan";
								break;
							case "туркменистан":
								user.HomeCountry = "Turkmenistan";
								break;
							case "узбекистан":
								user.HomeCountry = "Uzbekistan";
								break;
						}

						var country = _countryProvider.GetCountryByName(user.HomeCountry);
						if (country != null)
						{
							var city = cities.FirstOrDefault(s => s.CountryId == country.Id);
							if (city != null)
							{
								user.HomeCityId = city.Id;
								user.HomeCountryId = city.CountryId;
							}
						}
					}
					// если ничё не нашли - ну и хрен с ним.
				}
				// если заполнена только страна - сохраним хотя бы её
				else if(!string.IsNullOrEmpty(user.HomeCountry))
				{
					var country = _countryProvider.GetCountryByName(user.HomeCountry);
					if (country != null)
						user.HomeCountryId = country.Id;
				}
			}
		}

		public void UpdateUserName(string userId, string userName)
		{
			var objectId = MongoHelper.GetObjectIdFromString(userId, string.Format("{0} is not valid user id", userId));
			_userRepository.UpdateUserName(objectId, userName);
		}

		public string GetLastLocation(string userId)
		{
			var lastLocation = _userRepository.GetLastLocation(userId);
			if (lastLocation != null)
				return lastLocation.CityId;
			
			var user = GetUserById(userId);
			return !string.IsNullOrEmpty(user.HomeCityId) ? user.HomeCityId : string.Empty;
		}

		public void SaveLastLocation(string userId, string cityId)
		{
			var location = new UserLocation
				               {
					               UserId = userId,
								   CityId = cityId,
								   Time = DateTime.UtcNow
				               };

			_userRepository.SaveLastLocation(location);
		}
	}
}