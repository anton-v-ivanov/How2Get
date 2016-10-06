using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Web.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	public class CityController : ApiController
    {
        private readonly ICityProvider _cityProvider;

		public CityController(ICityProvider cityProvider)
		{
			_cityProvider = cityProvider;
		}

		// GET api/city/name
        [WebApiOutputCache(86400)]
        public IEnumerable<CityShortInfo> GetByName(string name)
        {
            if(string.IsNullOrEmpty(name))
                return new List<CityShortInfo>();

			var result = _cityProvider.SearchCity(name);

			if (result.Any(city => result.Any(s => s.CountryCode == city.CountryCode && s.Name == city.Name && s.Id != city.Id)))
		        return result.Select(city => new CityShortInfo(city, true));
			return result.Select(city => new CityShortInfo(city, false));
        }

		[WebApiOutputCache(86400)]
		public CityShortInfo GetById(string id)
		{
			return new CityShortInfo(_cityProvider.GetById(id), false);
		}
    }
}
