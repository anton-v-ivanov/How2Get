using System.Collections.Generic;
using System.Web.Http;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Web.Filters;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	[Authorize]
	public class CarrierController : ApiController
    {
        private readonly ICarrierProvider _carrierProvider;

		public CarrierController(ICarrierProvider carrierProvider)
		{
			_carrierProvider = carrierProvider;
		}

		// GET api/carrier
        public IEnumerable<Carrier> GetForCountry(string countryId, CarrierTypes type)
        {
			return _carrierProvider.GetForCountry(countryId, type);
        }

        // GET api/carrier?id=5
        public Carrier Get(string id)
        {
			return _carrierProvider.GetById(id);
        }

		public IEnumerable<Carrier> GetByName(string name, CarrierTypes type)
		{
			return _carrierProvider.GetByName(name, type, null);
		}

		public IEnumerable<Carrier> GetByName(string name, CarrierTypes type, string countryId)
		{
			return _carrierProvider.GetByName(name, type, countryId);
		}
    }
}
