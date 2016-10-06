using HowToGet.BusinessLogic.Validators;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface ICityValidator
	{
		void ValidateCity(CityValidator.ValidateCityType cityType, string cityId);
	}
}