using System.Collections.Generic;
using HowToGet.Models.Dictionaries;

namespace HowToGet.BusinessLogic.Interfaces
{
    public interface ICurrencyProvider
    {
        IEnumerable<Currency> GetAll();

        Currency GetById(int id);
        
		Currency GetByCode(string currencyCode);

        IEnumerable<Currency> GetForCountry(string countryId);
    }
}