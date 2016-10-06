using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Routes;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace HowToGet.Repository.Repositories
{
    public class RouteRepository : IRouteRepository
    {
	    private const string RoutesCollectionName = "routes";
	    private const string FindedRoutesCollectionName = "findedRoutes";
	    private const string FavoriteRoutesCollectionName = "favoriteRoutes";

	    public List<RoutePart> GetRoutePartsForOrigin(string originCityId)
		{
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
	        var cityQuery = Query.EQ("o", originCityId);
			var query = Query.And(Query.EQ("st", (int)RouteStatus.New),
									Query.ElemMatch("rp", cityQuery));
			var routes = collection.Find(query);
			return (from route in routes from routePart in route.RouteParts where routePart.OriginCityId == originCityId select routePart).ToList();
		}

		//public void DeleteRoute(ObjectId routeId)
		//{
		//	var collection = MongoHelper.Database.GetCollection<Route>("routes");
		//	var query = Query.EQ("_id", routeId);
		//	collection.Remove(query, RemoveFlags.Single);
		//}

		public void RemoveRoutePart(ObjectId routePartId)
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			var routePartQuery = Query.EQ("_id", routePartId);
			var query = Query.ElemMatch("rp", routePartQuery);
			var update = Update.Pull("rp", Query.EQ("_id", routePartId));
			collection.Update(query, update);
		}

	    public void UpdateRoute(Route route)
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			var query = Query.EQ("_id", ObjectId.Parse(route.Id));
			var update = Update<Route>.Replace(route);
		    collection.Update(query, update, UpdateFlags.Upsert);
	    }

	    public Route GetRouteByRoutePartId(ObjectId routePartId)
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			var routePartQuery = Query.EQ("_id", routePartId);
			var query = Query.ElemMatch("rp", routePartQuery);
		    return collection.FindOne(query);
	    }

	    public Route GetRouteById(ObjectId id)
        {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			var route = collection.FindOneById(id);
		    if (route.Status != RouteStatus.New)
			    return null;
		    return route;
        }

	    public void CreateRoute(Route route)
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			collection.Insert(route);
	    }

	    public IEnumerable<Route> GetRoutesForUser(ObjectId userId)
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			var query = Query.And(Query.EQ("st", (int)RouteStatus.New),
									Query.EQ("u", userId));
			return collection.Find(query);
		}

	    public void SaveFindedRoute(FindedRoute route)
	    {
			var collection = MongoHelper.Database.GetCollection<FindedRoute>(FindedRoutesCollectionName);
		    var id = ObjectId.Parse(route.Id);
			if (collection.FindOneById(id) != null)
		    {
			    var query = Query.EQ("_id", id);
			    var update = Update.Set("t", route.TimeStamp);
			    collection.Update(query, update);
		    }
		    else
			    collection.Insert(route);
	    }

	    public FindedRoute GetFindedRouteById(ObjectId routeId)
	    {
			var collection = MongoHelper.Database.GetCollection<FindedRoute>(FindedRoutesCollectionName);
			return collection.FindOneById(routeId);
		}

	    public IEnumerable<FindedRoute> GetFindedRoutes(string originCityId, string destinationCityId, DateTime minTimeStamp)
	    {
			var collection = MongoHelper.Database.GetCollection<FindedRoute>(FindedRoutesCollectionName);
			var query = Query.And(
					Query.EQ("o", originCityId),
					Query.EQ("d", destinationCityId),
					Query.GTE("t", minTimeStamp.Ticks)
			    );
			return collection.Find(query);
		}

	    public FavoriteRoutes GetFavoriteRoutes(ObjectId userId)
	    {
			var collection = MongoHelper.Database.GetCollection<FavoriteRoutes>(FavoriteRoutesCollectionName);
		    var query = Query.EQ("u", userId);
			return collection.FindOne(query);
		}

	    public void SaveFavoriteRoutes(FavoriteRoutes routes)
	    {
			var collection = MongoHelper.Database.GetCollection<FavoriteRoutes>(FavoriteRoutesCollectionName);
			var query = Query.EQ("u", routes.UserId);
		    var update = Update.Replace(routes);
		    collection.Update(query, update);
	    }

	    public void IncreaseRouteRank(ObjectId id)
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			var query = Query.EQ("_id", id);
			var update = Update.Inc("r", 1);
			collection.Update(query, update);
	    }

	    public IEnumerable<Route> GetTopRoutes(int count)
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
		    var query = Query.EQ("st", (int) RouteStatus.New);
			var sort = SortBy.Descending("r");
		    return collection.Find(query).SetSortOrder(sort).SetLimit(count);
	    }

	    public IEnumerable<string> GetTopUsers()
	    {
			var collection = MongoHelper.Database.GetCollection<Route>(RoutesCollectionName);
			var userIds = (from r in collection.AsQueryable() select r.UserId).ToList();
			return userIds.GroupBy(u => u)
					.Select(g => new { Value = g.Key, Count = g.Count()})
					.OrderByDescending(x=>x.Count)
					.Select(s => s.Value);
	    }
    }
}