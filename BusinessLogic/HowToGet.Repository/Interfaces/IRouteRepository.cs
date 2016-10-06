using System;
using System.Collections.Generic;
using HowToGet.Models.Routes;
using HowToGet.Models.Users;
using MongoDB.Bson;

namespace HowToGet.Repository.Interfaces
{
    public interface IRouteRepository
    {
		Route GetRouteById(ObjectId id);
	    
		void CreateRoute(Route route);
	    
		IEnumerable<Route> GetRoutesForUser(ObjectId userId);

		List<RoutePart> GetRoutePartsForOrigin(string originCityId);
	    
		//void DeleteRoute(ObjectId routeId);
	    
		void RemoveRoutePart(ObjectId routePartId);
	    
		void UpdateRoute(Route route);

		Route GetRouteByRoutePartId(ObjectId routePartId);

		void SaveFindedRoute(FindedRoute route);
	    
		FindedRoute GetFindedRouteById(ObjectId routeId);

		IEnumerable<FindedRoute> GetFindedRoutes(string originCityId, string destinationCityId, DateTime minTimeStamp);
	    
		FavoriteRoutes GetFavoriteRoutes(ObjectId userId);
	    
		void SaveFavoriteRoutes(FavoriteRoutes routes);
	    
		void IncreaseRouteRank(ObjectId id);
	    
		IEnumerable<Route> GetTopRoutes(int count);

		IEnumerable<string> GetTopUsers();
    }
}