using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Web.Filters;
using HowToGet.Web.Models;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	[Authorize]
	public class CurrencyController : ApiController
    {
        private readonly ICurrencyProvider _currencyProvider;

		public CurrencyController(ICurrencyProvider currencyProvider)
		{
			_currencyProvider = currencyProvider;
		}

		// GET api/currency
        [WebApiOutputCache(86400)]
        public IEnumerable<Currency> Get()
        {
			return _currencyProvider.GetAll();
        }

        // GET api/currency?id=5
        [WebApiOutputCache(86400)]
        public CurrencyShortData Get(int id)
        {
			var currency = _currencyProvider.GetById(id);
			return new CurrencyShortData(currency);
        }

        [WebApiOutputCache(86400)]
		public IEnumerable<CurrencyShortData> GetForCountry(string countryId)
        {
			var currencies = _currencyProvider.GetForCountry(countryId);
	        return currencies.Select(currency => new CurrencyShortData(currency));
        }
    }
}
