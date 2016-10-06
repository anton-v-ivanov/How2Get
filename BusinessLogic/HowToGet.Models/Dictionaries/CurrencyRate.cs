using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Dictionaries
{
	public class CurrencyRate
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("cf")]
		public int CurrencyFromId { get; set; }
		
		[BsonElement("ct")]
		public int CurrencyToId { get; set; }
		
		[BsonElement("r")]
		public double Rate { get; set; }

		[BsonElement("u")]
		public DateTime Updated { get; set; }
	}
}