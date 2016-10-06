using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Driver.Linq;

namespace HowToGet.Repository.Repositories
{
	public class SubscriptionsRepository : ISubscriptionsRepository
	{
		private const string CollectionName = "subscriptions";

		public IEnumerable<LaunchSubscription> GetAllSubscriptions(int omit, int count)
		{
			var collection = MongoHelper.Database.GetCollection<LaunchSubscription>(CollectionName);
			return collection.AsQueryable().OrderBy(s => s.Id).Skip(omit).Take(count);
		}
	}
}