using System.Collections.Generic;
using System.Linq;
using HowToGet.BusinessLogic.Helpers;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Utils;
using HowToGet.BusinessLogic.Utils.Unidecode;
using HowToGet.Models.Analytics;
using HowToGet.Repository.Interfaces;
using HowToGet.Models.Dictionaries;

namespace HowToGet.BusinessLogic.Providers
{
    public class CarrierProvider : ICarrierProvider
    {
        private readonly ICarrierRepository _carrierRepository;
	    private readonly IActionEvents _actionEvents;

	    public CarrierProvider(ICarrierRepository carrierRepository, IActionEvents actionEvents)
	    {
		    _carrierRepository = carrierRepository;
		    _actionEvents = actionEvents;
	    }

	    #region ICarrierProvider Members

		public IEnumerable<Carrier> GetForCountry(string countryId, CarrierTypes type)
        {
			var objectId = SystemHelper.GetObjectIdFromString(countryId, "{0} is not valid country id");
			return _carrierRepository.GetByCountryId(objectId, type);
        }

        public Carrier GetById(string id)
        {
			var objectId = SystemHelper.GetObjectIdFromString(id, "{0} is not valid carrier id");

			return _carrierRepository.GetById(objectId);
        }

	    public Carrier FindOneByName(string carrierName, CarrierTypes type)
	    {
			return _carrierRepository.FindByName(carrierName, type);
	    }

	    public string CreateCarrier(Carrier carrier, string userId)
	    {
		    carrier.LowercaseName = carrier.Name.ToLowerInvariant();
			_carrierRepository.CreateCarrier(carrier);

		    var action = new CarrierAddedAction(userId, carrier.Id);
			_actionEvents.OnUserAction(action);

		    return carrier.Id;
	    }
		
		public IEnumerable<Carrier> GetByName(string name, CarrierTypes type, string countryId)
		{
			// для авиаперевозчиков игнорируем страну
			if (type == CarrierTypes.Airway)
				countryId = string.Empty;

			var result = _carrierRepository.GetByName(name, type, countryId);
			if (result == null || !result.Any())
			{
				// транслитерация, если результат введён на языке, отличающимся от английского
				var translit = name.Unidecode();
				result = _carrierRepository.GetByName(translit, type, countryId);
			}
			return result;
		}

		//private IEnumerable<Carrier> FillCountryInfo(IEnumerable<Carrier> carriers)
		//{
		//	var result = new List<Carrier>();
		//	foreach (var carrier in carriers)
		//	{
		//		var country = CountryProvider.GetCountryById(carrier.CountryId);
		//		carrier.Country = country.Name;
		//		result.Add(carrier);
		//	}
		//	return result;
		//}

	    #endregion
    }
}