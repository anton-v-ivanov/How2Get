using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Routes
{
    public class Route
    {
	    [BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		
		[BsonElement("d")]
		[BsonIgnoreIfNull]
		public string Description { get; set; }
		
		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("u")]
		public string UserId { get; set; }

		[BsonElement("rp")]
		public List<RoutePart> RouteParts { get; set; }

		[BsonIgnore]
		public string UserName { get; set; }

		[BsonElement("cdt")]
		public DateTime CreationDateTime { get; set; }
		
		[BsonElement("udt")]
		public DateTime UpdatedDateTime { get; set; }

		[BsonElement("r")]
		public int Rank { get; set; }

		[BsonElement("st")]
		public RouteStatus Status { get; set; }

	    [BsonIgnore]
		public int TotalMinutes
        { 
            get
            {
                int totalTimeInMinutes = 0;
                if(RouteParts != null)
	                totalTimeInMinutes += RouteParts.Sum(routePart => routePart.Time);
	            return totalTimeInMinutes;
            }
        }

        public Route()
        {
            RouteParts = new List<RoutePart>();
        }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj is Route == false) return false;
			return Equals((Route) obj);
		}

		public bool Equals(Route other)
		{
			if (RouteParts.Count != other.RouteParts.Count)
				return false;

			if (RouteParts.Where((t, i) => !t.Equals(other.RouteParts[i])).Any())
			{
				return false;
			}
			return true;
		}
		
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (Id != null ? Id.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (UserId != null ? UserId.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (RouteParts != null ? RouteParts.GetHashCode() : 0);
				return hashCode;
			}
		}
    }
}
