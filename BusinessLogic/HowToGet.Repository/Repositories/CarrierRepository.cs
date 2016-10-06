using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace HowToGet.Repository.Repositories
{
	public class CarrierRepository : ICarrierRepository
	{
		private const string CollectionName = "carriers";

		public IEnumerable<Carrier> GetAll()
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			return collection.FindAll().AsEnumerable();
		}

		public Carrier GetById(ObjectId id)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			return collection.FindOneById(id);
		}

		public Carrier FindByName(string carrierName, CarrierTypes type)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			var query = Query.And(
					Query.EQ("ln", carrierName.ToLowerInvariant()),
					Query.EQ("t", Convert.ToInt32(type))
				);
			return collection.FindOne(query);
		}

		public void CreateCarrier(Carrier carrier)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			collection.Insert(carrier);
		}

		public IEnumerable<Carrier> GetByName(string name, CarrierTypes type, string countryId)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);

			string lowerName = name.ToLowerInvariant();

			if (string.IsNullOrWhiteSpace(countryId))
			{
				var result = collection.AsQueryable().Where(c => c.LowercaseName.StartsWith(lowerName) && c.Type == type).Take(10);
				return result.Any() ? result : collection.AsQueryable().Where(c => c.LowercaseName.Contains(lowerName) && c.Type == type).Take(10);
			}

			var result1 = collection.AsQueryable().Where(c => c.LowercaseName.StartsWith(lowerName) && c.Type == type && c.CountryId == countryId).Take(10);
			return result1.Any() ? result1 : collection.AsQueryable().Where(c => c.LowercaseName.Contains(lowerName) && c.Type == type && c.CountryId == countryId).Take(10);
		}

		public IEnumerable<Carrier> GetByCountryId(ObjectId countryId, CarrierTypes type)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			var query = Query.And(
					Query.EQ("ci", countryId),
					Query.EQ("t", Convert.ToInt32(type))
				);
			return collection.Find(query);
		}

		public void UpdateCountry(Carrier carrier)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			var query = Query.EQ("_id", ObjectId.Parse(carrier.Id));
			var update = Update.Set("ci", carrier.CountryId);
			collection.Update(query, update);
		}

		public void UpdateWeb(Carrier carrier)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			var query = Query.EQ("_id", ObjectId.Parse(carrier.Id));
			var update = Update.Set("w", carrier.Web);
			collection.Update(query, update);
		}

		public void UpdateIcon(Carrier carrier)
		{
			var collection = MongoHelper.Database.GetCollection<Carrier>(CollectionName);
			var query = Query.EQ("_id", ObjectId.Parse(carrier.Id));
			var update = Update.Set("i", carrier.Icon);
			collection.Update(query, update);
		}
	}
}