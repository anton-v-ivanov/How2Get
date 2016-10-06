using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class RoutesViewForUserAction : ActionBase
	{
		public override string UserId { get; set; }
		
		[BsonElement("fuid")]
		public string ForUserId { get; set; }

		[BsonElement("cnt")]
		public int RoutesCount { get; set; }

		public RoutesViewForUserAction(string currentUserId, string forUserId, int routesCount)
		{
			UserId = currentUserId;
			ForUserId = forUserId;
			RoutesCount = routesCount;
		}
	}
}