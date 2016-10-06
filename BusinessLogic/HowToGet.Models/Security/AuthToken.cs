using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Security
{
	public class AuthToken
	{
		public AuthToken(string userId, string token)
		{
			UserId = userId;
			Token = token;
		}

		public ObjectId Id { get; set; }

		[BsonElement("u")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }

		[BsonElement("t")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Token { get; set; }
	}
}