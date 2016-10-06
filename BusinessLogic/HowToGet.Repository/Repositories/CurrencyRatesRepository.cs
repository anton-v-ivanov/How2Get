using System;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class CurrencyRatesRepository : ICurrencyRatesRepository
	{
		private const string CollectionName = "currencyRates";

		public DateTime GetLastUpdated()
		{
			var collection = MongoHelper.Database.GetCollection<CurrencyRate>(CollectionName);
			var rate = collection.FindOne();
			return rate != null ? rate.Updated : DateTime.MinValue;
		}

		public void SaveCurrencyRate(CurrencyRate rate)
		{
			var collection = MongoHelper.Database.GetCollection<CurrencyRate>(CollectionName);
			var query = Query.And(
									Query.EQ("cf", rate.CurrencyFromId), 
									Query.EQ("ct", rate.CurrencyToId)
								);
			var update = Update.Set("r", rate.Rate).Set("u", rate.Updated);
			var result = collection.FindAndModify(query, SortBy.Null, update, true, false);
			if (result.ModifiedDocument == null)
				collection.Insert(rate);
		}

		public CurrencyRate GetRate(int currencyFromId, int currencyToId)
		{
			var collection = MongoHelper.Database.GetCollection<CurrencyRate>(CollectionName);
			var query = Query.And(
									Query.EQ("cf", currencyFromId),
									Query.EQ("ct", currencyToId)
								);
			return collection.FindOne(query);
		}
	}
}