using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Routes
{
	public class FindedRoute
	{
		public FindedRoute(Route route, string originCityId, string destinationCityId)
		{
			Id = route.Id;
			Origin = originCityId;
			Destination = destinationCityId;
			TimeStamp = DateTime.UtcNow.Ticks;
			PartIds = new List<string>();
			PartIds.AddRange(route.RouteParts.Select(s => s.Id));
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		
		[BsonElement("o")]
		public string Origin { get; set; }
		
		[BsonElement("d")]
		public string Destination { get; set; }
		
		[BsonElement("t")]
		public long TimeStamp { get; set; }

		[BsonElement("p")]
		public List<string> PartIds { get; set; }
	}
}