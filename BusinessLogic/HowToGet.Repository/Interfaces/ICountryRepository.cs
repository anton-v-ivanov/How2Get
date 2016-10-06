using System.Collections.Generic;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Repository.Interfaces
{
	public interface ICountryRepository
	{
		List<Country> GetAllCountries();
	}
}