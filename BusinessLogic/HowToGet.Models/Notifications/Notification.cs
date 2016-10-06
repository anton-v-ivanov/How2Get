using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Notifications
{
	public class Notification
	{
		public Notification(string userId, NotificationType type, object data)
		{
			UserId = userId;
			Type = type;
			Data = data;
			CreateTime = DateTime.UtcNow;
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("t")]
		public NotificationType Type { get; set; }

		[BsonElement("u")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }

		[BsonElement("r")]
		public bool IsRead { get; set; }
		
		[BsonElement("ct")]
		public DateTime CreateTime { get; set; }

		[BsonElement("d")]
		public object Data { get; set; }
	}
}