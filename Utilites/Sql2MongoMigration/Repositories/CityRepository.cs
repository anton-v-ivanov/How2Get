using System;
using BLToolkit.Data;
using HowToGet.Models.Dictionaries;
using System.Collections.Generic;

namespace Sql2MongoMigration.Repositories
{
    public class CityRepository
    {
		//internal IEnumerable<City> LoadAllCities()
		//{
		//	using (var db = new DbManager())
		//	{
		//		var cities = db
		//			.SetSpCommand("LoadAllCities")
		//			.ExecuteList<City>();
				
		//		foreach (var city in cities)
		//		{
		//			city.AlternateNamesList = new List<string>();
		//			var names = city.AlternateNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		//			foreach (var name in names)
		//			{
		//				city.AlternateNamesList.Add(name);
		//			}
		//		}
		//		return cities;
		//	}
		//}
    }
}
