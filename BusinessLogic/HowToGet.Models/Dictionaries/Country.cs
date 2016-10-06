using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Dictionaries
{
	public class Country
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		
		[BsonElement("n")]
		public string Name { get; set; }
		
		[BsonElement("cc")]
		public string CountryCode { get; set; }
	}
}