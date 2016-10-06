using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Security
{
	public enum OneTimeTokenState
	{
		Active = 0,
		Used = 1,
	}

	public class OneTimeToken
	{
		public ObjectId Id { get; set; }
		
		[BsonElement("u")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }
		
		[BsonElement("t")]
		public string Token { get; set; }
		
		[BsonElement("s")]
		public OneTimeTokenState State { get; set; }

		public OneTimeToken(string userId, string token)
		{
			UserId = userId;
			Token = token;
		}
	}
}