using System.Collections.Generic;
using HowToGet.Models.Routes;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class RouteAnnounceRepository : IRouteAnnounceRepository
	{
		private const string CollectionName = "routeAnnouncements";

		public void CreateSubscription(RouteUserSubscription subscription)
		{
			var collection = MongoHelper.Database.GetCollection<RouteUserSubscription>(CollectionName);
			collection.Save(subscription);
		}

		public void RemoveSubscription(RouteUserSubscription subscription)
		{
			var collection = MongoHelper.Database.GetCollection<RouteUserSubscription>(CollectionName);
			var query = Query.And(
							Query.EQ("o", subscription.Origin),
							Query.EQ("d", subscription.Destination),
							Query.EQ("e", subscription.Email)
							);
			collection.Remove(query);
		}

		public IEnumerable<RouteUserSubscription> GetSubscriptions(string origin, string destination)
		{
			var collection = MongoHelper.Database.GetCollection<RouteUserSubscription>(CollectionName);
			var query = Query.And(
							Query.EQ("o", origin),
							Query.EQ("d", destination)
							);
			return collection.FindAs<RouteUserSubscription>(query);
		}

		public RouteUserSubscription FindSubscription(string origin, string destination, string email, string userId)
		{
			var collection = MongoHelper.Database.GetCollection<RouteUserSubscription>(CollectionName);
			var query = Query.And(
							Query.EQ("o", origin), 
							Query.EQ("d", destination), 
							!string.IsNullOrEmpty(userId) 
								? Query.EQ("u", userId) 
								: Query.EQ("e", email));
			return collection.FindOne(query);
		}
	}
}