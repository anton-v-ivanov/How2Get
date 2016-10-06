using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Helpers;
using MongoDB.Driver.Linq;

namespace CountriesJoiner
{
	public class Repository
	{
		internal void InsertCity(City city)
		{
			var collection = MongoHelper.Database.GetCollection<HowToGet.Models.Dictionaries.City>("cities2");
			collection.Insert(city);
		}

		public void CreateCarrier(Carrier carrier)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>("carriers2");
			collection.Insert(carrier);
		}
		
		public IEnumerable<Carrier> GetByCountryName(string name)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>("carriers");
			return collection.AsQueryable().Where(c => c.Country == name);
		}

		public IEnumerable<Carrier> GetAllCarriers1()
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>("carriers");
			return collection.FindAll().AsEnumerable();
		}

		public IEnumerable<Carrier> GetAllCarriers2()
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>("carriers2");
			return collection.FindAll().AsEnumerable();
		}
	}
}