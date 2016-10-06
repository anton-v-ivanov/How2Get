using System.Collections.Generic;
using HowToGet.Models.Dictionaries;
using MongoDB.Bson;

namespace HowToGet.Repository.Interfaces
{
    public interface ICarrierRepository
    {
        IEnumerable<Carrier> GetAll();
        
        Carrier GetById(ObjectId id);

		Carrier FindByName(string carrierName, CarrierTypes type);
	    
		void CreateCarrier(Carrier carrier);

		IEnumerable<Carrier> GetByName(string name, CarrierTypes type, string countryId);

		IEnumerable<Carrier> GetByCountryId(ObjectId countryId, CarrierTypes type);
    }
}