using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Analytics;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Notifications;
using HowToGet.Models.Routes;
using HowToGet.Notifications.Interfaces;
using HowToGet.Repository.Interfaces;

namespace HowToGet.BusinessLogic.Providers
{
	public class RouteAnnounceProvider : IRouteAnnounceProvider
	{
		private readonly IRouteAnnounceRepository _repository;
		private readonly IEmailNotificationProvider _notificationProvider;
		private readonly ICityProvider _cityProvider;
		private readonly IActionEvents _actionEvents;
		private readonly IRouteProvider _routeProvider;

		public RouteAnnounceProvider(IRouteAnnounceRepository repository, IEmailNotificationProvider notificationProvider, ICityProvider cityProvider, IActionEvents actionEvents, IRouteProvider routeProvider)
		{
			_repository = repository;
			_notificationProvider = notificationProvider;
			_cityProvider = cityProvider;
			_actionEvents = actionEvents;
			_routeProvider = routeProvider;
			_actionEvents.UserAction += ActionEventsOnUserAction;
		}

		private void ActionEventsOnUserAction(ActionBase action)
		{
			if (!(action is RouteAddAction)) 
				return;

			var routeAddAction = action as RouteAddAction;
			var route = _routeProvider.GetRouteById(routeAddAction.RouteId);
			foreach (var part in route.RouteParts)
			{
				var subscribers = _repository.GetSubscriptions(part.OriginCityId, part.DestinationCityId).ToList();
				foreach (var subscriber in subscribers)
				{
					SendRouteAnnounce(subscriber, route);
				}
			}
		}

		private void SendRouteAnnounce(RouteUserSubscription subscriber, Route route)
		{
			throw new NotImplementedException();
		}

		public bool IsUserSubscribed(string origin, string destination, string userId)
		{
			var subscription = _repository.FindSubscription(origin, destination, null, userId);
			return subscription != null;
		}

		private bool IsSubscriptionExists(string origin, string destination, string email)
		{
			var subscription = _repository.FindSubscription(origin, destination, email, null);
			return subscription != null;
		}

		public IEnumerable<string> GetSubscribedUserIds(string origin, string destination, out int count)
		{
			var subscriptions = _repository.GetSubscriptions(origin, destination).ToList();
			
			count = subscriptions.Count();

			return subscriptions.Where(s => !string.IsNullOrEmpty(s.UserId))
			                     .Select(s => s.UserId);
		}

		public void AddSubscription(string origin, string destination, string email, string userId)
		{
			if (IsSubscriptionExists(origin, destination, email))
				return;

			var originCity = _cityProvider.GetById(origin);
			var destinationCity = _cityProvider.GetById(destination);

			if(originCity == null)
				throw new ValidationException(string.Format("{0} is not valid CityId", origin));
			if(destinationCity == null)
				throw new ValidationException(string.Format("{0} is not valid CityId", destination));

			var subscription = new RouteUserSubscription
			{
				Origin = origin,
				Destination = destination,
				Email = email.ToLowerInvariant(),
				UserId = userId
			};
			_repository.CreateSubscription(subscription);

			_notificationProvider.SendUnregistered(EmailNotificationType.RouteAnnounce, email, 
								new Dictionary<string, object>
									{
										{ "origin", originCity.Name }, 
										{ "destination", destinationCity.Name },
										{ "origin-id", originCity.Id}, 
										{ "destination-id", destinationCity.Id},
									});
		}

		public void RemoveSubscription(string origin, string destination, string email)
		{
			var subscription = new RouteUserSubscription
			{
				Origin = origin,
				Destination = destination,
				Email = email.ToLowerInvariant(),
			};
			_repository.RemoveSubscription(subscription);
		}
	}
}