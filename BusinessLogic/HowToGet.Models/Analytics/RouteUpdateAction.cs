using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class RouteUpdateAction : ActionBase
	{
		public override string UserId { get; set; }
		
		[BsonElement("rid")]
		public string RouteId { get; set; }

		public RouteUpdateAction(string userId, string routeId)
		{
			UserId = userId;
			RouteId = routeId;
		}
	}
}