using System.Collections.Generic;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Repository.Interfaces
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> GetAll();

        Currency GetById(int id);

        IEnumerable<Currency> GetForCountry(string countryId);
	    
		Currency GetByCode(string currencyCode);
    }
}