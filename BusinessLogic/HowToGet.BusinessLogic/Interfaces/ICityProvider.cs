using HowToGet.Models.Dictionaries;
using System.Collections.Generic;

namespace HowToGet.BusinessLogic.Interfaces
{
    public interface ICityProvider
    {
        /// <summary>
        /// City search by name.
        /// </summary>
        /// <param name="name">City name. Can be partial</param>
        /// <returns></returns>
        List<City> SearchCity(string name);

        bool IsValidCityId(string cityId);
	    
		City GetById(string id);

		void IncreaseCitiesRank(string originCityId, string destinationCityId);
	    
		void PreLoad();

	    City GetCityNear(double lat, double lng);
    }
}
