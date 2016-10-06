using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class RoutesViewForCurrentUserAction : ActionBase
	{
		public override string UserId { get; set; }
		
		[BsonElement("cnt")]
		public int RoutesCount { get; set; }

		public RoutesViewForCurrentUserAction(string userId, int routesCount)
		{
			UserId = userId;
			RoutesCount = routesCount;
		}
	}
}