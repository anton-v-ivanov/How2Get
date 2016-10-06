using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class RegisterAction : ActionBase
    {
		public override string UserId { get; set; }

		[BsonElement("ref")]
		[BsonIgnoreIfNull]
		public string Referrer { get; set; }

		[BsonElement("ip")]
		[BsonIgnoreIfNull]
		public string Ip { get; set; }

		public RegisterAction(string userId, string referrer, string ip)
		{
			UserId = userId;
			Referrer = referrer;
			Ip = ip;
		}
    }
}
