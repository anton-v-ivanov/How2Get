using System;
using HowToGet.Models.Dictionaries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Bonuses
{
	public class Bonus
	{
		public Bonus()
		{
		}
		
		public Bonus(string userId, BonusType bonusType)
		{
			UserId = userId;
			Type = bonusType;
			ReceivedTime = DateTime.UtcNow;
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("u")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string UserId { get; set; }

		[BsonElement("t")]
		public BonusType Type { get; set; }
		
		[BsonElement("rt")]
		public DateTime ReceivedTime { get; set; }
	}
}