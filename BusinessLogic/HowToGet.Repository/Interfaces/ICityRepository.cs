using HowToGet.Models.Dictionaries;
using System.Collections.Generic;
using MongoDB.Bson;

namespace HowToGet.Repository.Interfaces
{
    public interface ICityRepository
    {
        IEnumerable<City> LoadAllCities();

		void IncreaseCityRank(ObjectId cityId);
	}
}
