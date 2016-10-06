using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class RouteAddAction : ActionBase
	{
		public override string UserId { get; set; }
		
		[BsonElement("rid")]
		public string RouteId { get; set; }

		public RouteAddAction(string userId, string routeId)
		{
			UserId = userId;
			RouteId = routeId;
		}
	}
}