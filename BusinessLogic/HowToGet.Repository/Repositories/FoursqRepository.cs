using System.Linq;
using HowToGet.Models.Foursq;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class FoursqRepository : IFoursqRepository
	{
		const string CollectionName = "foursq";

		public bool IsCheckinExists(string id)
		{
			var collection = MongoHelper.Database.GetCollection<CheckinInfo>(CollectionName);
			return collection.FindOne(Query.EQ("fsqid", id)) != null;
		}

		public CheckinInfo GetUserLastCheckin(string userId)
		{
			var collection = MongoHelper.Database.GetCollection<CheckinInfo>(CollectionName);
			var query = Query.EQ("uid", userId);
			
			var checkin = collection.Find(query).SetSortOrder(SortBy.Descending("t")).SetLimit(1);
			return checkin.FirstOrDefault();
		}

		public void SaveCheckin(CheckinInfo checkin)
		{
			var collection = MongoHelper.Database.GetCollection<CheckinInfo>(CollectionName);
			collection.Save(checkin);
		}
	}
}