using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public abstract class ActionBase
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("u")]
		[BsonIgnoreIfNull]
		public abstract string UserId { get; set; }

		[BsonElement("t")]
		public virtual DateTime ActionTime 
		{
			get { return DateTime.UtcNow; }
		}
	}
}