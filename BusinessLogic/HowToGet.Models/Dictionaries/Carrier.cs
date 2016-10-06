using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Dictionaries
{
    public class Carrier
    {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("n")]
        public string Name { get; set; }
		
		[BsonElement("ln")]
		public string LowercaseName { get; set; }

		[BsonElement("t")]
		[BsonIgnoreIfNull]
	    public CarrierTypes Type { get; set; }

		[BsonElement("i")]
		[BsonIgnoreIfNull]
        public string Icon { get; set; }

		[BsonElement("d")]
		[BsonIgnoreIfNull]
        public string Description { get; set; }

		[BsonElement("ci")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string CountryId { get; set; }

		[BsonElement("w")]
		[BsonIgnoreIfNull]
		public string Web { get; set; }
    }
}