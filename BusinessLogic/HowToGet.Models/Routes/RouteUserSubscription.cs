using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Routes
{
	public class RouteUserSubscription
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("o")]
		public string Origin { get; set; }

		[BsonElement("d")]
		public string Destination { get; set; } 

		[BsonElement("e")]
		public string Email { get; set; } 

		[BsonElement("u")]
		[BsonIgnoreIfDefault]
		[BsonIgnoreIfNull]
		public string UserId { get; set; } 
	}
}