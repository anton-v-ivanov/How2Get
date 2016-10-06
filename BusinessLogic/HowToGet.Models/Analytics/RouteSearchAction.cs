using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class RouteSearchAction : ActionBase
    {
		public override string UserId { get; set; }

		[BsonElement("o")]
		public string Origin { get; set; }

		[BsonElement("d")]
		public string Destination { get; set; }

		[BsonElement("rc")]
		public int ResultCount { get; set; }

		public RouteSearchAction(string userId, string origin, string destination, int resultCount)
		{
			UserId = userId;
			Origin = origin;
			Destination = destination;
			ResultCount = resultCount;
		}
    }
}
