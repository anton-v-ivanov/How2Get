using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine
{
    public class RouteEngine : IRouteEngine
    {
		private readonly IRouteSolver _routeSolver;
		

		public RouteEngine(IRouteSolver routeSolver)
		{
			_routeSolver = routeSolver;
		}

		public IEnumerable<Route> FindRoute(string originCityId, string destinationCityId, ICollection<CarrierTypes> allowedCarrierTypes, int maxResultCount, int maxTransferCount)
        {
            var result = new List<Route>();
			var resultParts = _routeSolver.FindRoute(originCityId, destinationCityId, allowedCarrierTypes, maxResultCount, maxTransferCount);
			
			// reverse route solve
			if(resultParts.Count == 0)
				resultParts = _routeSolver.FindRoute(destinationCityId, originCityId, allowedCarrierTypes, maxResultCount, maxTransferCount);

            foreach (var resultPath in resultParts)
            {
                var route = new Route();
                route.RouteParts.AddRange(resultPath);
	            
				string routeId = resultPath[0].GlobalRouteId;

				// маршрут точно соответствует запрошенному
	            if (resultPath.All(routePart => routeId == routePart.GlobalRouteId))
	            {
		            route.Id = routeId;
	            }

	            result.Add(route);
            }
		    return result;
        }
    }
}