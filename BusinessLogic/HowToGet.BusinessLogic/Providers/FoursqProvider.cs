using System;
using System.Collections.Generic;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Foursq;
using HowToGet.Models.Notifications;
using HowToGet.Notifications.Interfaces;
using HowToGet.Repository.Interfaces;
using NLog;

namespace HowToGet.BusinessLogic.Providers
{
	public class FoursqProvider : IFoursqProvider
	{
		private readonly IFoursqRepository _repository;
		private readonly IUserProvider _userProvider;
		private readonly ICityProvider _cityProvider;
		private readonly IEmailNotificationProvider _emailNotificationProvider;
		private readonly IOneTimeTokenProvider _oneTimeTokenProvider;
		private readonly string _foursqPushSecret;

		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}
		
		public FoursqProvider(IFoursqRepository repository, IUserProvider userProvider, ICityProvider cityProvider, IEmailNotificationProvider emailNotificationProvider, IOneTimeTokenProvider oneTimeTokenProvider, string foursqPushSecret)
		{
			_repository = repository;
			_userProvider = userProvider;
			_cityProvider = cityProvider;
			_emailNotificationProvider = emailNotificationProvider;
			_oneTimeTokenProvider = oneTimeTokenProvider;
			_foursqPushSecret = foursqPushSecret;
		}

		public bool IsCheckinExists(string id)
		{
			return _repository.IsCheckinExists(id);
		}

		public void ProcessPush(FoursqPush push)
		{
			if(!push.Secret.Equals(_foursqPushSecret, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityException(string.Format("Invalid Foursquare secret {0}", push.Secret));

			var userId = _userProvider.GetExternalUserId(push.Checkin.User.Id, ExternalAuthServices.FourSquare);
			if (string.IsNullOrEmpty(userId))
			{
				CurrentLogger.Error(string.Format("Unknown external 4SQ_UserId: {0}", push.Checkin.User.Id));
				return;
			}
			
			var user = _userProvider.GetUserById(userId);
			if (user == null)
			{
				CurrentLogger.Error(string.Format("User external id is known but user was not found. UserId: {0}, 4SQ_UserId: {1}", userId, push.Checkin.User.Id));
				return;
			}

			var checkinInfo = GetCheckinInfo(push.Checkin, userId);
				
			var lastLocationCityId = _userProvider.GetLastLocation(userId);
			
			_repository.SaveCheckin(checkinInfo);
				
			if (string.IsNullOrEmpty(lastLocationCityId) || string.IsNullOrEmpty(checkinInfo.CityId))
			{
				return;
			}

			if (!lastLocationCityId.Equals(checkinInfo.CityId, StringComparison.InvariantCultureIgnoreCase))
			{
				_userProvider.SaveLastLocation(userId, checkinInfo.CityId);

				var lastCity = _cityProvider.GetById(lastLocationCityId);
				var currentCity = _cityProvider.GetById(checkinInfo.CityId);
				var authToken = _oneTimeTokenProvider.Generate(userId);

				_emailNotificationProvider.Send(EmailNotificationType.TripHappened, user, 
					new Dictionary<string, object>{ 
						{ "origin", lastCity.Name }, 
						{ "origin-id", lastCity.Id }, 
						{ "destination", currentCity.Name }, 
						{ "destination-id", currentCity.Id }, 
						{ "token", authToken }
					});
			}
		}

		private CheckinInfo GetCheckinInfo(FoursqCheckin checkin, string userId)
		{
			double? lat = null, lon = null;
			City city = null;
			if (checkin.Venue != null)
			{
				if (checkin.Venue.Location != null)
					city = ParseLocation(checkin.Venue.Location, out lat, out lon);
			}
			else if(checkin.Location != null)
			{
				city = ParseLocation(checkin.Location, out lat, out lon);
			}

			return new CheckinInfo
				       {
					       FoursqCheckinId = checkin.Id,
						   Time = DateTime.UtcNow,
						   UserId = userId,
						   Latitude = lat,
						   Longitude = lon,
						   CityId = city != null ? city.Id : null
				       };
		}

		private City ParseLocation(FoursqLocation location, out double? lat, out double? lng)
		{
			if (!string.IsNullOrEmpty(location.City) && !string.IsNullOrEmpty(location.Country))
			{
				var cities = _cityProvider.SearchCity(location.City);
				if (cities.Count == 1)
				{
					var city = cities[0];
					lat = city.Latitude;
					lng = city.Longitude;
					return city;
				}

				foreach (var city in cities)
				{
					if (city.CountryName.Equals(location.Country, StringComparison.InvariantCultureIgnoreCase))
					{
						lat = city.Latitude;
						lng = city.Longitude;
						return city;
					}
				}
			}

			if (location.Lat != 0 && location.Lng != 0)
			{
				lat = location.Lat;
				lng = location.Lng;
				return _cityProvider.GetCityNear(lat.Value, lng.Value);
			}
			

			lat = null;
			lng = null;
			return null;
		}
	}
}