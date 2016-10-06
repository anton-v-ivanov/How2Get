using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class RouteViewAction : ActionBase
	{
		public override string UserId { get; set; }
		
		[BsonElement("rid")]
		public string RouteId { get; set; }

		public RouteViewAction(string userId, string routeId)
		{
			UserId = userId;
			RouteId = routeId;
		}
	}
}