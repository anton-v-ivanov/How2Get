using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
	    private const string CollectionName = "currencies";

	    private const int EuroId = 43;
		private const int USDollarId = 143;

	    public IEnumerable<Currency> GetAll()
        {
			var collection = MongoHelper.Database.GetCollection<Currency>(CollectionName);
			return collection.FindAll().AsEnumerable();
		}

        public Currency GetById(int id)
        {
			var collection = MongoHelper.Database.GetCollection<Currency>(CollectionName);
			return collection.FindOneById(id);
		}

		public Currency GetByCode(string currencyCode)
		{
			var collection = MongoHelper.Database.GetCollection<Currency>(CollectionName);
			var query = Query.EQ("CurrencyCode", currencyCode.ToUpperInvariant());
			return collection.FindOne(query);
		}

        public IEnumerable<Currency> GetForCountry(string countryId)
        {
			var collection = MongoHelper.Database.GetCollection<Currency>(CollectionName);
	        var query = Query.EQ("ci", countryId);
			var currencies = collection.Find(query);
	        var result = new List<Currency>();
			foreach (var currency in currencies)
	        {
				result.Add(currency);
		        if(currency.Id != EuroId)
					result.Add(GetById(EuroId));
				if (currency.Id != USDollarId)
					result.Add(GetById(USDollarId));
	        }
	        return result;
        }

	    //public void UpdateCurrency(Currency currency)
		//{
		//	var collection = MongoHelper.Database.GetCollection<Currency>("currencies");
		//	var query = Query.EQ("_id", currency.Id);
		//	var update = Update
		//					.Set("ci", new BsonArray(currency.Countries))
		//					.Unset("CountryIds");
		//	collection.Update(query, update);
		//}
    }
}