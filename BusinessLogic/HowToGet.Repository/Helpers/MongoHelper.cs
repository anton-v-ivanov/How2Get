using System.Collections.Generic;
using System.Configuration;
using HowToGet.Models.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HowToGet.Repository.Helpers
{
	public static class MongoHelper
	{
		public static MongoDatabase Database { get; private set; }

		static MongoHelper()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["mongo"].ConnectionString;
			var client = new MongoClient(connectionString);
			var server = client.GetServer();
			Database = server.GetDatabase("howtoget");
		}

		public static void EnsureIndexes(Dictionary<string, string[]> collectionsIndexes)
		{
			foreach (var collection in collectionsIndexes)
			{
				var c = Database.GetCollection(collection.Key);
				foreach (var index in collection.Value)
				{
					c.EnsureIndex(index);	
				}
			}
		}
		
		public static ObjectId GetObjectIdFromString(string id, string errorMessage)
		{
			ObjectId objectId;
			if (!ObjectId.TryParse(id, out objectId))
				throw new InvalidObjectIdException(string.Format(errorMessage, id));

			return objectId;
		}
	}
}