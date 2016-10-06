using HowToGet.Models.Dictionaries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Users
{
	public class ExternalUserLink
	{
		public ObjectId Id { get; set; }

		[BsonElement("extId")]
		public string ExternalId { get; set; }

		[BsonElement("at")]
		public string AccessToken { get; set; }

		[BsonElement("uId")]
		public ObjectId UserId { get; set; }
		
		[BsonElement("as")]
		public ExternalAuthServices AuthService { get; set; }

		public ExternalUserLink(ObjectId userId, string externalId, ExternalAuthServices authService, string accessToken)
		{
			UserId = userId;
			ExternalId = externalId;
			AuthService = authService;
			AccessToken = accessToken;
		}
	}
}