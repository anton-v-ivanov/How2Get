using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Exceptions;
using HowToGet.Repository.Interfaces;

namespace HowToGet.BusinessLogic.Providers
{
	public class CountryProvider : ICountryProvider
	{
		private static List<Country> Countries { get; set; }
		
		private readonly ICountryRepository _countryRepository;

		public CountryProvider(ICountryRepository countryRepository)
		{
			_countryRepository = countryRepository;
			Countries = _countryRepository.GetAllCountries();
		}

		public Country GetCountryById(string id)
		{
			var country = Countries.FirstOrDefault(c => string.Equals(c.Id, id, StringComparison.InvariantCultureIgnoreCase));
			if(country != null)
				return country;
			
			throw new ObjectNotFoundException(string.Format("Unable to find country with id = {0}", id));
		}

		public Country GetCountryByName(string name)
		{
			return Countries.FirstOrDefault(s => string.Equals(s.Name, name, StringComparison.InvariantCultureIgnoreCase));
		}

		public bool IsCountryExists(string id)
		{
			return Countries.Exists(c => string.Equals(c.Id, id, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}