using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Exceptions;

namespace HowToGet.BusinessLogic.Validators
{
	public class CountryValidator : ICountryValidator
	{
		private readonly ICountryProvider _countryProvider;

		public CountryValidator(ICountryProvider countryProvider)
		{
			_countryProvider = countryProvider;
		}

		public void ValidateCountry(string countryId)
		{
			if(_countryProvider.IsCountryExists(countryId) == false)
				throw new ObjectNotFoundException("Country was not found");
		}
	}
}