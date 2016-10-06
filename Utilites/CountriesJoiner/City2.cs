using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CountriesJoiner
{
	public class City
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("n")]
		public string Name { get; set; }

		[BsonElement("anl")]
		public List<string> AlternateNamesList { get; set; }

		[BsonElement("lat")]
		public double Latitude { get; set; }

		[BsonElement("lon")]
		public double Longitude { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		[BsonElement("ci")]
		public string CountryId { get; set; }

		[BsonIgnore]
		public string CountryCode { get; set; }

		[BsonIgnore]
		public string CountryName { get; set; }

		[BsonElement("p")]
		public long Population { get; set; }

		[BsonElement("t")]
		public string Timezone { get; set; }

		[BsonElement("s")]
		public string StateName { get; set; }

		[BsonElement("r")]
		public long Rank { get; set; }

		public City(HowToGet.Models.Dictionaries.City city, string countryId)
		{
			Name = city.Name;
			AlternateNamesList = city.AlternateNamesList;
			Latitude = city.Latitude;
			Longitude = city.Longitude;
			CountryId = countryId;
			Population = city.Population;
			Timezone = city.Timezone;
			StateName = city.StateName;
		}
	}
}