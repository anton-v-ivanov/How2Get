using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Foursq
{
	public class CheckinInfo
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("fsqid")]
		public string FoursqCheckinId { get; set; }

		[BsonElement("uid")]
		public string UserId { get; set; }

		[BsonElement("t")]
		public DateTime Time { get; set; }

		[BsonElement("lat")]
		[BsonIgnoreIfNull]
		public double? Latitude { get; set; }

		[BsonElement("lon")]
		[BsonIgnoreIfNull]
		public double? Longitude { get; set; }

		[BsonElement("cid")]
		public string CityId { get; set; }
	}
}