using System.Collections.Generic;
using HowToGet.Models.Bonuses;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class BonusRepository : IBonusRepository
	{
		private const string CollectionName = "bonuses";

		public void AddBonus(Bonus bonus)
		{
			var collection = MongoHelper.Database.GetCollection<Bonus>(CollectionName);
			collection.Insert(bonus);
		}

		public void Remove(ObjectId id)
		{
			var collection = MongoHelper.Database.GetCollection<Bonus>(CollectionName);
			var query = Query.EQ("_id", id);
			collection.Remove(query);
		}

		public IEnumerable<Bonus> GetBonuses(ObjectId userId)
		{
			var collection = MongoHelper.Database.GetCollection<Bonus>(CollectionName);
			var query = Query.EQ("u", userId);
			return collection.Find(query);
		}
	}
}