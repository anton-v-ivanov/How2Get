using System.Collections.Generic;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Routes;

namespace HowToGet.BusinessLogic.Interfaces
{
    public interface IRouteProvider
    {
		IEnumerable<Route> FindRoute(string originCityId, string destinationCityId, ICollection<CarrierTypes> allowedCarrierTypes, RouteSortTypes sortType, int maxResultCount, int maxTransferCount);
        
        Route GetRouteById(string id);
	    
		string CreateRoute(Route route);
	    
		IEnumerable<Route> GetRoutesForUser(string userId);
	    
		void DeleteRoute(string routeId, string userId);

		Route RemoveRoutePart(string routePartId, string userId);

		Route UpdateRoute(Route route);
	    
		void MarkRouteAsFavorite(string routeId, string userId);

		List<Route> GetFavoriteRoutesForUser(string userId);
	    
		void RemoveFromFavorite(string routeId, string userId);

		IEnumerable<ShortRouteInfo> GetTopRoutes();
	    
		int CalculateRouteTime(string originId, string destId, CarrierTypes carrierType);
    }
}