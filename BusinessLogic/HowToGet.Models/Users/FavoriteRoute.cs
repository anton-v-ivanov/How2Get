using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Users
{
	public class FavoriteRoutes
	{
		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("u")]
		public string UserId { get; set; }
		
		[BsonElement("r")]
		public List<string> RouteIds { get; set; }
	}
}