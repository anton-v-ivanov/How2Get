using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Users
{
	public class UserEmail
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("f")]
		public string From { get; set; }

		[BsonElement("t")]
		public string To { get; set; }

		[BsonElement("s")]
		public string Subject { get; set; }

		[BsonElement("b")]
		public string Body { get; set; } 
	}
}