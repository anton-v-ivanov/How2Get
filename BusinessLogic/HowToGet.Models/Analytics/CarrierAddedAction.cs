using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Analytics
{
	public sealed class CarrierAddedAction : ActionBase
    {
		public override string UserId { get; set; }

		[BsonElement("cid")]
		public string CarrierId { get; set; }

		public CarrierAddedAction(string userId, string carrierId)
		{
			UserId = userId;
			CarrierId = carrierId;
		}
    }
}
