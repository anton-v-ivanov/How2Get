using System;
using System.Collections.Generic;
using BLToolkit.Data;
using HowToGet.BusinessLogic.Providers;
using Sql2MongoMigration.Repositories;


namespace Sql2MongoMigration
{
	class Program
	{
		static void Main(string[] args)
		{
			DbManager.AddConnectionString(@"Server=.\sql1;Database=howtoget;Integrated Security=SSPI");

			//var cityRepository = new CityRepository();
			//Console.WriteLine("Getting cities from SQL...");
			//var cities = cityRepository.LoadAllCities().ToList();
			//Console.WriteLine("{0} cities loaded", cities.Count());
			//var cityCollection = MongoHelper.Database.GetCollection<City>("cities");
			//int counter = 0;
			//foreach (var city in cities)
			//{
			//	counter++;
			//	if (counter % 100 == 0)
			//		Console.WriteLine("{0} cities added to MongoDB", counter);
			//	cityCollection.Insert(city);
			//}
			//Console.WriteLine("{0} cities loaded to MongoDB", counter);

			//var currencyRepository = new CurrencyRepository();
			//Console.WriteLine("Getting currencies from SQL...");
			//var currencies = currencyRepository.GetAll().ToList();
			//Console.WriteLine("{0} currencies loaded", currencies.Count());
			
			//var currencyCollection = MongoHelper.Database.GetCollection<Currency>("currencies");
			//counter = 0;
			//foreach (var currency in currencies)
			//{
			//	counter++;
			//	if (counter % 10 == 0)
			//		Console.WriteLine("{0} currencies added to MongoDB", counter);
			//	currencyCollection.Insert(currency);
			//}
			//Console.WriteLine("{0} currencies loaded to MongoDB", counter);

			var currencyRepository = new HowToGet.Repository.Repositories.Mongo.CurrencyRepository();
			var currencies = currencyRepository.GetAll();
			foreach (var currency in currencies)
			{
				if (currency.Countries == null)
					currency.Countries = new List<string>();
				
				if (currency.CountryIds != null)
				{
					foreach (var countryId in currency.CountryIds)
					{
						var countryName = CountryRepository.GetNameByIdFromSql(countryId);
						var id = CountryRepository.GetCountryIdFromMongo(countryName);
						currency.Countries.Add(id);
					}

					currencyRepository.UpdateCurrency(currency);
				}
			}

			Console.ReadKey();
		}
	}
}
