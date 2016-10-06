using System.Collections.Generic;
using HowToGet.Models.Dictionaries;

namespace HowToGet.BusinessLogic.Interfaces
{
    public interface ICarrierProvider
    {
		IEnumerable<Carrier> GetForCountry(string countryId, CarrierTypes type);

        Carrier GetById(string id);

		Carrier FindOneByName(string carrierName, CarrierTypes type);

		string CreateCarrier(Carrier carrier, string userId);

		IEnumerable<Carrier> GetByName(string name, CarrierTypes type, string countryId);
    }
}