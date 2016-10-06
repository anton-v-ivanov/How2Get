using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace HowToGet.Models.Dictionaries
{
    public class Currency : IEquatable<Currency>
    {
        public int Id { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencyName { get; set; }

		[BsonElement("ci")]
		public List<string> CountryIds { get; set; }

	    public bool Equals(Currency other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}