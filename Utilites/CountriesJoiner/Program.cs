using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Repositories.Mongo;
using NLog;

namespace CountriesJoiner
{
	class Program
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		static void Main()
		{
			Log.Info("Program started");

			var cityRepository = new CityRepository();
			var localRepository = new Repository();
			var cities = cityRepository.LoadAllCities().ToList();
			var countries = new List<Country>();
			var countryRepository = new CountryRepository();
			int globalCarriersCount = 0;


			foreach (var city in cities)
			{
				Log.Info("City: {0}", city.Name);
				Country country;
				if (!countries.Exists(s => s.Name == city.CountryName))
				{
					Log.Info("New country {0}", city.CountryName);

					country = new Country
									  {
										  CountryCode = city.CountryCode,
										  Name = city.CountryName
									  };
					countryRepository.InsertCountry(country);
					
					countries.Add(country);

					Log.Info("Country {0} added with Id = {1}", city.CountryName, country.Id);

					var carriers = localRepository.GetByCountryName(country.Name).ToList();

					Log.Info("Found {0} carriers in country {1}", carriers.Count, city.CountryName);
					globalCarriersCount += carriers.Count;

					foreach (var carrier in carriers)
					{
						var carrier2 = new Carrier
										   {
											   Name = carrier.Name,
											   CountryId = country.Id,
											   Description = carrier.Description,
											   Icon = carrier.Icon,
											   LowercaseName = carrier.LowercaseName,
											   Type = carrier.Type,
											   Web = carrier.Web
										   };

						localRepository.CreateCarrier(carrier2);
						Log.Info("Carrier {0} added with Id = {1} and Country = {2}", carrier2.Name, carrier2.Id, carrier2.CountryId);
					}
				}
				else
				{
					Log.Info("Country {0} already added", city.CountryName);
					country = countries.First(s => s.Name == city.CountryName);
				}

				var city2 = new City(city, country.Id);
				localRepository.InsertCity(city2);

				Log.Info("City2 added with Id = {1}", city.CountryName, city2);

			}

			Log.Info("{0} country, {1} cities and {2} carriers processed", countries.Count, cities.Count, globalCarriersCount);	

			var carriers1 = localRepository.GetAllCarriers1().ToList();
			Log.Info("Carriers1 count = {0}", carriers1.Count);

			var carriers2 = localRepository.GetAllCarriers2().ToList();
			Log.Info("Carriers2 count = {0}", carriers2.Count);

			foreach (var carrier1 in carriers1)
			{
				var c2 = carriers2.FirstOrDefault(s => s.Name == carrier1.Name);
				if (c2 == null)
					Log.Warn("Carrier not found: Id = {0}, Name = {1}, Country = {2}", carrier1.Id, carrier1.Name, carrier1.Country);
			}
		}
	}
}
