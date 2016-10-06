using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HowToGet.BusinessLogic.Configuration;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Validators;
using HowToGet.Models.Analytics;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Routes;
using HowToGet.Web.Filters;
using HowToGet.Web.Models;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	public class RouteController : ApiController
    {
        private readonly IRouteProvider _routeProvider;
		private readonly IRouteValidator _routeValidator;
		private readonly ICityValidator _cityValidator;
		private readonly IActionEvents _actionEvents;

		public RouteController(IRouteProvider routeProvider, IRouteValidator routeValidator, ICityValidator cityValidator, IActionEvents actionEvents)
		{
			_routeProvider = routeProvider;
			_routeValidator = routeValidator;
			_cityValidator = cityValidator;
			_actionEvents = actionEvents;
		}


		// GET api/route/?origin=id&destination=id
        public IEnumerable<Route> GetRoutes(string origin, string destination)
        {
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Origin, origin);
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Destination, destination);

			var allowedCarrierTypes = new List<CarrierTypes>
				                          {
					                          CarrierTypes.Airway,
											  CarrierTypes.Bus,
											  CarrierTypes.Car,
											  CarrierTypes.Ferry,
											  CarrierTypes.Railway,
											  CarrierTypes.Taxi
				                          };

	        return GetRoutes(origin, destination, allowedCarrierTypes, RouteSortTypes.NotSet);
        }

		private IEnumerable<Route> GetRoutes(string origin, string destination, List<CarrierTypes> allowedCarrierTypes, RouteSortTypes sortType)
		{
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Origin, origin);
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Destination, destination);

			if(allowedCarrierTypes == null)
				allowedCarrierTypes = new List<CarrierTypes>
				                {
					                CarrierTypes.Airway,
									CarrierTypes.Bus,
									CarrierTypes.Car,
									CarrierTypes.Ferry,
									CarrierTypes.Railway,
									CarrierTypes.Taxi
				                };

			var result = _routeProvider.FindRoute(origin, destination, allowedCarrierTypes, sortType, SearchConfig.Instance.MaxResultCount, SearchConfig.Instance.MaxTransferCount).ToList();

			var userId = User.Identity.Name;

			var action = new RouteSearchAction(userId, origin, destination, result.Count());
			_actionEvents.OnUserAction(action);
			
			return result;
		}

		// GET api/route/?id=id
		public Route GetRouteById(string id)
		{
			var result = _routeProvider.GetRouteById(id);

			var userId = User.Identity.Name;

			var action = new RouteViewAction(userId, id);
			_actionEvents.OnUserAction(action);

			return result;
		}

		[HttpGet]
		[Authorize]
		public IEnumerable<Route> GetRoutesForCurrentUser()
		{
			var userId = User.Identity.Name;
			var result = _routeProvider.GetRoutesForUser(userId).ToList();

			var action = new RoutesViewForCurrentUserAction(userId, result.Count());
			_actionEvents.OnUserAction(action);

			return result;
		}

		[ActionName("user")]
		[HttpGet]
		public IEnumerable<Route> GetRoutesForUser(string userId)
		{
			var result = _routeProvider.GetRoutesForUser(userId).ToList();

			var currentUserId = User.Identity.Name;

			var action = new RoutesViewForUserAction(currentUserId, userId, result.Count());
			_actionEvents.OnUserAction(action);

			return result;
		}

		// PUT api/route
		[HttpPost]
		[Authorize]
		public RouteCreateResultModel Add(Route route)
		{
			_routeValidator.ValidateRoute(route);
			var userId = User.Identity.Name;
			route.UserId = userId;
			var id = _routeProvider.CreateRoute(route);

			var action = new RouteAddAction(userId, route.Id);
			_actionEvents.OnUserAction(action);

			return new RouteCreateResultModel
				       {
					       RouteId = id
				       };
        }

		[HttpDelete]
		[Authorize]
		public void DeleteRoute(string id)
		{
			var userId = User.Identity.Name;
			_routeProvider.DeleteRoute(id, userId);
			
			var action = new RouteDeleteAction(userId, id);
			_actionEvents.OnUserAction(action);
		}

		[ActionName("removepart")]
		[HttpPatch]
		[Authorize]
		public Route RemoveRoutePart(string id)
		{
			var userId = User.Identity.Name;
			var result =  _routeProvider.RemoveRoutePart(id, userId);

			var action = new RouteUpdateAction(userId, result.Id);
			_actionEvents.OnUserAction(action);

			return result;
		}

		[ActionName("update")]
		[HttpPut]
		[Authorize]
		public Route UpdateRoute(Route route)
		{
			var userId = User.Identity.Name;
			route.UserId = userId;
			var result = _routeProvider.UpdateRoute(route);

			var action = new RouteUpdateAction(userId, result.Id);
			_actionEvents.OnUserAction(action);

			return result;
		}

		
		[ActionName("top")]
		[HttpGet]
		[WebApiOutputCache(10800)]
		public IEnumerable<ShortRouteInfo> GetTopRoutes()
		{
			return _routeProvider.GetTopRoutes();
		}

		[ActionName("calc")]
		[HttpGet]
		[WebApiOutputCache(10800)]
		public CalculatedRouteDataModel CalculateRouteData(string originId, string destinationId, CarrierTypes carrierType)
		{
			int time = _routeProvider.CalculateRouteTime(originId, destinationId, carrierType);
			return new CalculatedRouteDataModel
				       {
					       Time = time
				       };
		}
    }
}
