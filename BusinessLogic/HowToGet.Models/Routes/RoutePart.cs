using System;
using HowToGet.Models.Dictionaries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Routes
{
    public class RoutePart
    {
	    [BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		
		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("g")]
		public string GlobalRouteId { get; set; }
		
		[BsonElement("o")]
        public string OriginCityId { get; set; }
		
		[BsonIgnore]
        public CityShortInfo OriginCityInfo { get; set; }

		[BsonElement("d")]
		public string DestinationCityId { get; set; }
		
		[BsonIgnore]
        public CityShortInfo DestinationCityInfo { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("c")]
		[BsonIgnoreIfNull]
		public string CarrierId { get; set; }

		[BsonElement("dsc")]
		[BsonIgnoreIfNull]
		public string Description { get; set; }
		
		[BsonElement("t")]
		public int Time { get; set; }
		
		[BsonElement("cst")]
		[BsonIgnoreIfNull]
		public double Cost { get; set; }

		[BsonElement("cc")]
		[BsonIgnoreIfNull]
		public int CostCurrency { get; set; }

		[BsonElement("dt")]
		[BsonIgnoreIfNull]
		public DateTime? Date { get; set; }
		
		[BsonElement("udt")]
		[BsonIgnoreIfNull]
		public DateTime UpdateTime { get; set; }
		
		[BsonIgnore]
        public string CostCurrencyCode { get; set; }
		
		[BsonIgnore]
        public string CarrierName { get; set; }
		
		[BsonIgnore]
        public CarrierTypes CarrierType { get; set; }
		
		[BsonIgnore]
        public string CarrierIcon { get; set; }
		
		[BsonIgnore]
        public string CarrierDescription { get; set; }

		[BsonIgnore]
		public string CarrierUrl { get; set; }

		[BsonIgnore]
		public string UserId { get; set; }

		[BsonIgnore]
		public string UserName { get; set; }

	    public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (RoutePart)) return false;
			return Equals((RoutePart) obj);
		}
		
		public bool Equals(RoutePart other)
		{
			if(OriginCityId != other.OriginCityId || DestinationCityId != other.DestinationCityId)
				return false;
			if(!string.Equals(CarrierId, other.CarrierId))
				return false;
			if(Time != other.Time)
				return false;
			if(!Cost.Equals(other.Cost) || CostCurrency != other.CostCurrency)
				return false;
			
			return true;
		}
		
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = OriginCityId.GetHashCode();
				hashCode = (hashCode * 397) ^ DestinationCityId.GetHashCode();
				hashCode = (hashCode * 397) ^ (CarrierId != null ? CarrierId.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Time;
				hashCode = (hashCode * 397) ^ Cost.GetHashCode();
				hashCode = (hashCode * 397) ^ CostCurrency;
				return hashCode;
			}
		}
    }
}
