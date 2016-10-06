using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Users
{
	public class UserLocation
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("uid")]
		public string UserId { get; set; }

		[BsonElement("cid")]
		public string CityId { get; set; }

		[BsonElement("t")]
		public DateTime Time { get; set; }
	}
}