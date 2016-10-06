using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Exceptions;

namespace HowToGet.BusinessLogic.Validators
{
	public class CityValidator : ICityValidator
	{
		private readonly ICityProvider _cityProvider;

		public CityValidator(ICityProvider cityProvider)
		{
			_cityProvider = cityProvider;
		}

		public enum ValidateCityType
		{
			Origin,
			Destination
		}

		public void ValidateCity(ValidateCityType cityType, string cityId)
		{
			if (string.IsNullOrEmpty(cityId))
			{
				if (cityType == ValidateCityType.Origin)
					throw new ValidationException("Origin point is not specified");
				throw new ValidationException("Destination point is not specified");
			}

			if (!_cityProvider.IsValidCityId(cityId))
			{
				if (cityType == ValidateCityType.Origin)
					throw new ObjectNotFoundException("Origin point was not found");
				throw new ObjectNotFoundException("Destination point was not found");
			}
		}
	}
}