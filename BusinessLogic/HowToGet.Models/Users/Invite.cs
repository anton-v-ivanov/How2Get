using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Users
{
	public enum InviteStatus
	{
		Generated = 0,
		Used = 1,
		Sended = 2,
	}

	public class Invite
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("i")]
		public string InviteCode { get; set; }

		[BsonElement("g")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string GeneratedUserId { get; set; }
		
		[BsonElement("u")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string UsedByUserId { get; set; }

		[BsonElement("s")]
		public InviteStatus Status { get; set; }

		[BsonElement("st")]
		public string SendedTo { get; set; }
	}
}