using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;

namespace HowToGet.Repository.Repositories
{
	public class CountryRepository : ICountryRepository
	{
		private const string CollectionName = "countries";

		public List<Country> GetAllCountries()
		{
			var collection = MongoHelper.Database.GetCollection<Country>(CollectionName);
			return collection.FindAll().ToList();
		}

		public void InsertCountry(Country country)
		{
			var collection = MongoHelper.Database.GetCollection<Country>(CollectionName);
			collection.Insert(country);
		}
	}
}