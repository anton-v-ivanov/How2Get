using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HowToGet.BusinessLogic.Helpers;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Exceptions;
using HowToGet.Repository.Interfaces;
using HowToGet.Models.Dictionaries;
using System;
using HowToGet.RouteEngine;

namespace HowToGet.BusinessLogic.Providers
{
	public class CityProvider : ICityProvider
	{
		private readonly ICityRepository _cityRepository;
		private readonly ICountryProvider _countryProvider;
		private readonly int _searchResultCount;

		private static ILookup<string, string> CitiesNames { get; set; }

		public static readonly object SyncObj = new object();

		private static ConcurrentDictionary<string, City> _allCities;
		private ConcurrentDictionary<string, City> AllCities 
		{ 
			get
			{
				if (_allCities == null)
					LoadAllCities();
				
				return _allCities;
			} 
		}

		public CityProvider(ICityRepository cityRepository, ICountryProvider countryProvider, int searchResultCount)
		{
			_cityRepository = cityRepository;
			_countryProvider = countryProvider;
			_searchResultCount = searchResultCount;
		}

		private void LoadAllCities()
		{
			lock (SyncObj)
			{
				_allCities = new ConcurrentDictionary<string, City>();
				var cityList = _cityRepository.LoadAllCities();
				var namesTempList = new List<KeyValuePair<string, string>>();
				foreach (var city in cityList)
				{
					var country = _countryProvider.GetCountryById(city.CountryId);
					city.CountryName = country.Name;
					city.CountryCode = country.CountryCode;

					_allCities.AddOrUpdate(city.Id, city, (s, c) => city);

					namesTempList.Add(new KeyValuePair<string, string>(city.Name, city.Id));
					namesTempList.AddRange(city.AlternateNamesList.Select(name => new KeyValuePair<string, string>(name, city.Id)));
				}
				CitiesNames = namesTempList.OrderBy(s => s.Key).ToLookup(k => k.Key, v => v.Value);
			}
		}

		public List<City> SearchCity(string name)
		{
			var result = new List<City>(_searchResultCount);
			var resultedCities = new List<string>(_searchResultCount);
			foreach (var namePair in CitiesNames)
			{
				if (namePair.Key.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
				{
					foreach (var cityId in namePair)
					{
						if (!resultedCities.Contains(cityId))
						{
							result.Add(AllCities[cityId]);
							resultedCities.Add(cityId);
						}
					}
				}
			}
			return SortResult(result).Take(_searchResultCount).ToList();
		}

		public bool IsValidCityId(string cityId)
		{
			return AllCities.ContainsKey(cityId);
		}

		public City GetById(string id)
		{
			City result;
			if (AllCities.TryGetValue(id, out result))
				return result;
			throw new ObjectNotFoundException(string.Format("City with Id = {0} was not found", id));
		}

		public void IncreaseCitiesRank(string originCityId, string destinationCityId)
		{
			var t1 = new Task(() => IncreaseCityRank(originCityId));
			var t2 = new Task(() => IncreaseCityRank(destinationCityId));
			t1.Start();
			t2.Start();
		}

		public void PreLoad()
		{
			LoadAllCities();
		}

		public City GetCityNear(double lat, double lng)
		{
			double minLatDiff = Double.MaxValue, minLngDiff = double.MaxValue;
			string resultCityId = string.Empty;

			foreach (var city in AllCities.Values)
			{
				var latDiff = Math.Abs(city.Latitude - lat);
				var lngDiff = Math.Abs(city.Longitude - lng);
				if (latDiff < minLatDiff && lngDiff < minLngDiff)
				{
					minLatDiff = latDiff;
					minLngDiff = lngDiff;
					resultCityId = city.Id;
					//if (DistanceCalculator.GetDistance(lat, lng, city.Latitude, city.Longitude) < 1000)
					//	break;
				}
			}
			return !string.IsNullOrEmpty(resultCityId) ? AllCities[resultCityId] : null;
		}

		private void IncreaseCityRank(string cityId)
		{
			AllCities[cityId].Rank++;
			_cityRepository.IncreaseCityRank(SystemHelper.GetObjectIdFromString(cityId, "{0} is not valid city id"));
		}

		private static IEnumerable<City> SortResult(IEnumerable<City> list)
		{
			return list.OrderByDescending(s => s.Rank).ThenByDescending(s => s.Population);
		}
	}
}
