using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Interfaces;

namespace HowToGet.BusinessLogic.Providers
{
    public class CurrencyProvider : ICurrencyProvider
    {
        private readonly ICurrencyRepository _currencyRepository;
	    private readonly Currency _euroCurrency;
	    private readonly Currency _usdCurrency;
	    private static List<Currency> _allCurrencies;
	    private readonly int _euroCurrencyCode;
	    private readonly int _usdCurrencyCode;

	    public CurrencyProvider(ICurrencyRepository currencyRepository, int euroCurrencyCode, int usdCurrencyCode)
	    {
		    _currencyRepository = currencyRepository;
		    _allCurrencies = GetAll().ToList();
			_euroCurrencyCode = euroCurrencyCode;
		    _usdCurrencyCode = usdCurrencyCode;
		    _euroCurrency = GetById(euroCurrencyCode);
		    _usdCurrency = GetById(usdCurrencyCode);
	    }

	    public IEnumerable<Currency> GetAll()
        {
			return _currencyRepository.GetAll();
        }

        public Currency GetById(int id)
        {
	        var result = _allCurrencies.FirstOrDefault(s => s.Id == id);
	        return result ?? _currencyRepository.GetById(id);
        }

	    public Currency GetByCode(string currencyCode)
	    {
			var result = _allCurrencies.FirstOrDefault(s => s.CurrencyCode.Equals(currencyCode, StringComparison.InvariantCultureIgnoreCase));
			return result ?? _currencyRepository.GetByCode(currencyCode);
		}

	    public IEnumerable<Currency> GetForCountry(string countryId)
        {
	        var currencies = _allCurrencies.Where(s => s.CountryIds.Contains(countryId)).ToList();
		    if (!currencies.Any())
			    currencies = _currencyRepository.GetForCountry(countryId).ToList();

			var result = new List<Currency>();
			result.AddRange(currencies);
			if(!result.Exists(s => s.Id == _usdCurrencyCode))
				result.Add(_usdCurrency);
			
			if (!result.Exists(s => s.Id == _euroCurrencyCode))
				result.Add(_euroCurrency);

	        return result;
        }
    }
}