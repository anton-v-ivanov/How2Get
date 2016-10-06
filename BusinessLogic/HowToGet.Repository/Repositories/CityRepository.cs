using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class CityRepository : ICityRepository
	{
		private const string CollectionName = "cities";

		public IEnumerable<City> LoadAllCities()
		{
			var collection = MongoHelper.Database.GetCollection<City>(CollectionName);
			return collection.FindAll().AsEnumerable();
		}

		public void IncreaseCityRank(ObjectId cityId)
		{
			var collection = MongoHelper.Database.GetCollection<City>(CollectionName);
			var query = Query.EQ("_id", cityId);
			var update = Update.Inc("r", 1);
			collection.Update(query, update);
		}

		//public List<City> Search(string name)
		//{
		//	var collection = MongoHelper.Database.GetCollection<City>("cities");
		//	var query = Query.Or(
		//							Query.Matches("Name", name),
		//							Query.Matches("AlternateNamesList", name)
		//						);
		//	return collection.Find(query).SetSortOrder(SortBy.Descending("Rank").Descending("Population")).ToList();
		//}
	}
}
