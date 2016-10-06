using BLToolkit.Data;
using HowToGet.Repository.Interfaces;
using HowToGet.Models.Dictionaries;
using System.Collections.Generic;

namespace HowToGet.Repository.Providers
{
    public class CityRepository : ICityRepository
    {
        public IEnumerable<City> LoadAllCities()
        {
            using (var db = new DbManager())
            {
                return db
                    .SetSpCommand("LoadAllCities")
                    .ExecuteList<City>();
            }
        }
    }
}
