using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HowToGet.BusinessLogic.Configuration;
using HowToGet.BusinessLogic.Helpers;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Repository.Interfaces;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine;
using HowToGet.RouteEngine.Interfaces;
using MongoDB.Bson;

namespace HowToGet.BusinessLogic.Providers
{
    public class RouteProvider : IRouteProvider
	{
		#region Providers

		private readonly IRouteRepository _routeRepository;
		private readonly ICarrierProvider _carrierProvider;
		private readonly ICityProvider _cityProvider;
		private readonly ICurrencyProvider _currencyProvider;
		private readonly IUserProvider _userProvider;
		private readonly IRouteEngine _routeEngine;
		private readonly IRouteRanker _routeRanker;
	    private readonly int _routeCacheTime;

	    #endregion


	    private static readonly object LockObj = new object();

	    public RouteProvider(IRouteRepository routeRepository, ICarrierProvider carrierProvider, ICityProvider cityProvider, ICurrencyProvider currencyProvider, 
								IUserProvider userProvider, IRouteEngine routeEngine, IRouteRanker routeRanker, int routeCacheTime)
	    {
		    _routeRepository = routeRepository;
		    _carrierProvider = carrierProvider;
		    _cityProvider = cityProvider;
		    _currencyProvider = currencyProvider;
		    _userProvider = userProvider;
		    _routeEngine = routeEngine;
			_routeRanker = routeRanker;
		    _routeCacheTime = routeCacheTime;
	    }

	    private const int AvgAirSpeed = 900;
		private const int AvgRailSpeed = 70;
		private const int AvgFerrySpeed = 40;
		private const int AvgCarSpeed = 60;

        #region IRouteProvider Members

		public IEnumerable<Route> FindRoute(string originCityId, string destinationCityId, ICollection<CarrierTypes> allowedCarrierTypes, RouteSortTypes sortType, int maxResultCount, int maxTransferCount)
        {
			var minTimeStamp = DateTime.UtcNow.AddMinutes(-_routeCacheTime);
			var findedRoutes = _routeRepository.GetFindedRoutes(originCityId, destinationCityId, minTimeStamp);

	        var result = findedRoutes.Select(ConvertFindedRouteToRoute)
							.Where(route => route != null 
								&& route.RouteParts.All(s => allowedCarrierTypes.Contains(s.CarrierType)))
							.ToList();
			if (result.Count >= maxResultCount)
			{
				return _routeRanker.SortResults(result.Take(maxResultCount), sortType);
			}
	        var resultCount = maxResultCount - result.Count;

	        var newResult = _routeEngine.FindRoute(originCityId, destinationCityId, allowedCarrierTypes, resultCount, maxTransferCount).ToList();
			foreach (var route in newResult)
			{
				if (string.IsNullOrEmpty(route.Id))
				{
					lock (LockObj)
					{
						route.Id = ObjectId.GenerateNewId().ToString();
					}
					
				}

				if (!HasEqualRoute(result, route))
				{
					_routeRepository.SaveFindedRoute(new FindedRoute(route, originCityId, destinationCityId));
					FillRouteData(route);
					result.Add(route);
				}
			}
			
			_cityProvider.IncreaseCitiesRank(originCityId, destinationCityId);
	        IncreaseRoutesRank(result);

	        return _routeRanker.SortResults(result, sortType);
        }

	    public Route GetRouteById(string id)
        {
			var objectId = SystemHelper.GetObjectIdFromString(id, "{0} is not valid route id");

            var route = _routeRepository.GetRouteById(objectId);
	        if (route == null)
	        {
		        var findedRoute = _routeRepository.GetFindedRouteById(objectId);
		        if (findedRoute == null)
			        return null;

		        route = ConvertFindedRouteToRoute(findedRoute);
				if (route == null)
					return null;
	        }
	        FillRouteData(route);
	        return route;
        }

	    private Route ConvertFindedRouteToRoute(FindedRoute findedRoute)
	    {
		    var route = new Route {Id = findedRoute.Id};
		    foreach (var partId in findedRoute.PartIds)
		    {
			    var tempRoute = _routeRepository.GetRouteByRoutePartId(SystemHelper.GetObjectIdFromString(partId, "{0} is not valid route part id"));
			    if (tempRoute == null)
				    return null;

			    var routePart = tempRoute.RouteParts.FirstOrDefault(s => s.Id == partId);
			    if (routePart == null)
				    return null;

			    route.RouteParts.Add(routePart);
		    }
			FillRouteData(route);
		    return route;
	    }

		private void FillRouteData(Route route)
		{
			var loadedRoutes = new Dictionary<string, Route>();

			bool needToFetchUsers = string.IsNullOrEmpty(route.UserId);
			if (!needToFetchUsers)
			{
				var user = _userProvider.GetUserById(route.UserId);
				route.UserName = user.Username;
			}

			if(route.UpdatedDateTime == DateTime.MinValue)
				route.UpdatedDateTime = route.RouteParts.Max(s => s.UpdateTime);

			if (needToFetchUsers)
			{
				var globalRouteId = route.RouteParts[0].GlobalRouteId;
				if (route.Id == globalRouteId && route.RouteParts.All(r => r.GlobalRouteId == globalRouteId))
				{
					var tmpRoute = _routeRepository.GetRouteById(ObjectId.Parse(globalRouteId));
					if (tmpRoute != null)
					{
						route.UserId = tmpRoute.UserId;
						var user = _userProvider.GetUserById(route.UserId);
						route.UserName = user.Username;
						needToFetchUsers = false;
						loadedRoutes.Add(globalRouteId, tmpRoute);
					}
				}
			}

			foreach (var routePart in route.RouteParts)
			{
				routePart.OriginCityInfo = new CityShortInfo(_cityProvider.GetById(routePart.OriginCityId));
				routePart.DestinationCityInfo = new CityShortInfo(_cityProvider.GetById(routePart.DestinationCityId));
				if (routePart.CostCurrency != 0)
				{
					var currency = _currencyProvider.GetById(routePart.CostCurrency);
					if (currency == null)
						throw new ObjectNotFoundException(
							string.Format("Unable to find currency with id {0} for routeId = {1}, routePartId={2}", routePart.CostCurrency,
							              route.Id, routePart.Id));
					routePart.CostCurrencyCode = currency.CurrencyCode;
				}
				if (!string.IsNullOrEmpty(routePart.CarrierId))
				{
					var carrier = _carrierProvider.GetById(routePart.CarrierId);
					if (carrier == null)
						throw new ObjectNotFoundException(string.Format("Unable to find carrier with id {0} for routeId = {1}, routePartId={2}", routePart.CarrierId, route.Id, routePart.Id));

					routePart.CarrierName = carrier.Name;
					routePart.CarrierDescription = carrier.Description;
					routePart.CarrierIcon = carrier.Icon;
					routePart.CarrierType = carrier.Type;
					routePart.CarrierUrl = carrier.Web;
				}

				if (needToFetchUsers)
				{
					if (!loadedRoutes.ContainsKey(routePart.GlobalRouteId))
					{
						ObjectId routeId;
						if (ObjectId.TryParse(routePart.GlobalRouteId, out routeId))
						{
							var loadedRoute = _routeRepository.GetRouteById(routeId);
							loadedRoutes.Add(routePart.GlobalRouteId, loadedRoute);
							routePart.UserId = loadedRoute.UserId;
						}
					}
					else
						routePart.UserId = loadedRoutes[routePart.GlobalRouteId].UserId;

					var user = _userProvider.GetUserById(routePart.UserId);
					if (user != null)
						routePart.UserName = user.Username;
				}
				else
				{
					routePart.UserId = route.UserId;
					routePart.UserName = route.UserName;
				}
			}
		}

	    private static bool HasEqualRoute(ICollection<Route> findedRoutes, Route newRoute)
		{
			if (findedRoutes.Count == 0)
				return false;

			return findedRoutes.Any(findedRoute => findedRoute.Equals(newRoute));
		}

	    public string CreateRoute(Route route)
		{
			route.CreationDateTime = DateTime.UtcNow;
			route.UpdatedDateTime = DateTime.UtcNow;
		    route.Status = RouteStatus.New;

		    lock (LockObj)
		    {
				route.Id = ObjectId.GenerateNewId().ToString();
		    }
			
			foreach (var routePart in route.RouteParts)
			{
				FillRoutePartOnCreation(routePart, route);
			}
			_routeRepository.CreateRoute(route);
			return route.Id;
		}

		private void FillRoutePartOnCreation(RoutePart routePart, Route route)
		{
			if (string.IsNullOrWhiteSpace(routePart.CarrierId))
			{
				routePart.CarrierId = FindOrCreateCarrier(routePart, route.UserId);
			}
			else
			{
				var carrier = _carrierProvider.GetById(routePart.CarrierId);
				if(carrier == null)
					throw new InvalidObjectIdException(string.Format("{0} is not valid carrier id. RouteId = {1}", routePart.CarrierId, route.Id));
			}
			if (string.IsNullOrWhiteSpace(routePart.Id))
			{
				lock (LockObj)
				{
					routePart.Id = ObjectId.GenerateNewId().ToString();
				}
			}
			if(string.IsNullOrWhiteSpace(routePart.GlobalRouteId))
				routePart.GlobalRouteId = route.Id;
			
			routePart.UpdateTime = DateTime.UtcNow;
		}

	    private string FindOrCreateCarrier(RoutePart routePart, string userId)
	    {
		    if (string.IsNullOrEmpty(routePart.CarrierName))
		    {
			    routePart.CarrierName = string.Empty;
		    }
			var carrier = _carrierProvider.FindOneByName(routePart.CarrierName, routePart.CarrierType);
			if (carrier == null)
			{
				carrier = new Carrier
				{
					Name = routePart.CarrierName,
					Type = routePart.CarrierType,
				};

				carrier.Id = _carrierProvider.CreateCarrier(carrier, userId);
			}
			return carrier.Id;
		}

	    public IEnumerable<Route> GetRoutesForUser(string userId)
	    {
			var objectId = SystemHelper.GetObjectIdFromString(userId, "{0} is not valid user id");

		    var routes = _routeRepository.GetRoutesForUser(objectId).ToList();
		    foreach (var route in routes)
			    FillRouteData(route);

		    return routes;
	    }

		public void DeleteRoute(string routeId, string userId)
	    {
			var objectId = SystemHelper.GetObjectIdFromString(routeId, "{0} is not valid route id");
			
			var route = _routeRepository.GetRouteById(objectId);
			if(route == null)
				throw new ObjectNotFoundException(string.Format("Unable to find route with id = {0}", routeId));

			if(route.UserId != userId)
				throw new SecurityException(string.Format("Route with id = {0} does not belong to current user", routeId));
			
			route.Status = RouteStatus.Deleted;
		    _routeRepository.UpdateRoute(route);
	    }

		public Route RemoveRoutePart(string routePartId, string userId)
	    {
			var objectId = SystemHelper.GetObjectIdFromString(routePartId, "{0} is not valid routePart id");
			
			var route = _routeRepository.GetRouteByRoutePartId(objectId);
			if (route == null)
				throw new ObjectNotFoundException(string.Format("Unable to find route with route part id = {0}", routePartId));

			if (route.UserId != userId)
				throw new SecurityException(string.Format("Route with id = {0} does not belong to current user", route.Id));

			_routeRepository.RemoveRoutePart(objectId);
			return GetRouteById(route.Id);
	    }

	    public Route UpdateRoute(Route route)
	    {
		    var prevRoute = GetRouteById(route.Id);
			
			if(prevRoute == null)
				throw new ObjectNotFoundException(string.Format("Unable to find route with route part id = {0}", route.Id));

			if (prevRoute.UserId != route.UserId)
				throw new SecurityException(string.Format("Route with id = {0} does not belong to current user", route.Id));

			route.UpdatedDateTime = DateTime.UtcNow;
		    route.CreationDateTime = prevRoute.CreationDateTime;

			foreach (var routePart in route.RouteParts)
			{
				FillRoutePartOnCreation(routePart, route);
				routePart.UserId = route.UserId;
			}

			//if (route.Description != null)
			//	prevRoute.Description = route.Description;
			//foreach (var routePart in route.RouteParts)
			//{
			//	var prevPart = prevRoute.RouteParts.FirstOrDefault(s => s.Id == routePart.Id);
			//	if (prevPart == null)
			//	{
			//		FillRoutePart(routePart, route.Id);
			//		prevRoute.RouteParts.Add(routePart);
			//	}
			//	else
			//	{
			//		if (!string.IsNullOrEmpty(routePart.OriginCityId) && prevPart.OriginCityId != routePart.OriginCityId)
			//		{
			//			if(_cityProvider.IsValidCityId(routePart.OriginCityId))
			//				prevPart.OriginCityId = routePart.OriginCityId;
			//			else
			//				throw new InvalidObjectIdException(string.Format("{0} is not valid original city id. RouteId = {1}", routePart.OriginCityId, route.Id));
			//		}
					
			//		if (!string.IsNullOrEmpty(routePart.DestinationCityId) && prevPart.DestinationCityId != routePart.DestinationCityId)
			//		{
			//			if (_cityProvider.IsValidCityId(routePart.DestinationCityId))
			//				prevPart.DestinationCityId = routePart.DestinationCityId;
			//			else
			//				throw new InvalidObjectIdException(string.Format("{0} is not valid destination city id. RouteId = {1}", routePart.DestinationCityId, route.Id));
			//		}
				    
			//		if (routePart.CarrierId != null && prevPart.CarrierId != routePart.CarrierId)
			//		{
			//			var carrier = _carrierProvider.GetById(routePart.CarrierId);
			//			if (carrier == null)
			//				throw new InvalidObjectIdException(string.Format("{0} is not valid carrier id. RouteId = {1}", routePart.CarrierId, route.Id));
			//			prevPart.CarrierId = routePart.CarrierId;
			//		}

			//		if (routePart.CarrierName != null && prevPart.CarrierName != routePart.CarrierName)
			//		{
			//			prevPart.CarrierId = FindOrCreateCarrier(routePart);
			//		}

			//		if (routePart.Description != null)
			//			prevPart.Description = routePart.Description;

			//		if (routePart.Time != 0)
			//			prevPart.Time = routePart.Time;

			//		if (routePart.Cost != -1)
			//			prevPart.Cost = routePart.Cost;

			//		if (routePart.CostCurrency != 0)
			//		{
			//			var currency = _currencyProvider.GetById(routePart.CostCurrency);
			//			if(currency == null)
			//				throw new InvalidObjectIdException(string.Format("{0} is not valid currency id. RouteId = {1}", routePart.CostCurrency, route.Id));
						
			//			prevPart.CostCurrency = routePart.CostCurrency;
			//		}

			//		if (routePart.Date != null)
			//			prevPart.Date = routePart.Date;
			//	}
		    //}

		    _routeRepository.UpdateRoute(route);
		    return route;
	    }

	    public void MarkRouteAsFavorite(string routeId, string userId)
	    {
		    var routes = GetFavoriteRoutes(userId) ?? new FavoriteRoutes();
		    if (routes.RouteIds.Contains(routeId)) 
				return;

		    routes.RouteIds.Add(routeId);
		    _routeRepository.SaveFavoriteRoutes(routes);
	    }

	    private FavoriteRoutes GetFavoriteRoutes(string userId)
		{
			var objectId = SystemHelper.GetObjectIdFromString(userId, "{0} is not valid user id");
			return _routeRepository.GetFavoriteRoutes(objectId);
		}

	    public List<Route> GetFavoriteRoutesForUser(string userId)
	    {
		    var routes = GetFavoriteRoutes(userId);
		    if (routes == null)
			    return null;

		    return routes.RouteIds.Select(GetRouteById).Where(route => route != null).ToList();
	    }

	    public void RemoveFromFavorite(string routeId, string userId)
	    {
			var routes = GetFavoriteRoutes(userId);
		    if (routes == null)
			    return;
		    
			routes.RouteIds.Remove(routeId);
			_routeRepository.SaveFavoriteRoutes(routes);
	    }

		private void IncreaseRoutesRank(IEnumerable<Route> result)
		{
			foreach (var route in result)
			{
				var id = ObjectId.Parse(route.Id);
				var t1 = new Task(() => _routeRepository.IncreaseRouteRank(id));
				t1.Start();
			}
		}

	    public IEnumerable<ShortRouteInfo> GetTopRoutes()
	    {
		    const int count = 5;
		    var routes = _routeRepository.GetTopRoutes(count);
		    var result = new List<ShortRouteInfo>();
		    foreach (var route in routes)
		    {
			    var shortRouteInfo = new ShortRouteInfo
				                         {
					                         Id = route.Id, 
											 UserId = route.UserId,
											 OriginCityId = route.RouteParts.First().OriginCityId,
											 DestinationCityId = route.RouteParts.Last().DestinationCityId,
				                         };

			    var user = _userProvider.GetUserById(route.UserId);
				if (user != null)
					shortRouteInfo.UserName = user.Username;
			    
				shortRouteInfo.OriginCityInfo = new CityShortInfo(_cityProvider.GetById(shortRouteInfo.OriginCityId));
				shortRouteInfo.DestinationCityInfo = new CityShortInfo(_cityProvider.GetById(shortRouteInfo.DestinationCityId));
				
				result.Add(shortRouteInfo);
		    }
		    return result;
	    }

	    public int CalculateRouteTime(string originId, string destId, CarrierTypes carrierType)
	    {
			SystemHelper.GetObjectIdFromString(originId, "{0} is not valid city id");
			SystemHelper.GetObjectIdFromString(destId, "{0} is not valid city id");
		    
			var origin = _cityProvider.GetById(originId);
			var destination = _cityProvider.GetById(destId);

			var distance = DistanceCalculator.GetDistance(origin.Latitude, origin.Longitude,
											  destination.Latitude, destination.Longitude);

		    int avgSpeed;
		    switch (carrierType)
		    {
			    case CarrierTypes.Railway:
				    avgSpeed = AvgRailSpeed;
				    break;

			    case CarrierTypes.Airway:
				    avgSpeed = AvgAirSpeed;
				    break;

			    case CarrierTypes.Ferry:
					avgSpeed = AvgFerrySpeed;
				    break;

			    case CarrierTypes.Bus:
			    case CarrierTypes.Car:
			    case CarrierTypes.Taxi:
					avgSpeed = AvgCarSpeed;
				    break;
			    
				default:
				    throw new ArgumentOutOfRangeException("carrierType");
		    }
			
			var time = (distance / avgSpeed) * 60;
		    return Convert.ToInt32(time);
	    }

	    #endregion
    }
}