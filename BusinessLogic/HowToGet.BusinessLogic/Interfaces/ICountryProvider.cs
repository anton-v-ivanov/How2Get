using HowToGet.Models.Dictionaries;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface ICountryProvider
	{
		Country GetCountryById(string id);
		
		Country GetCountryByName(string name);

		bool IsCountryExists(string id);
	}
}