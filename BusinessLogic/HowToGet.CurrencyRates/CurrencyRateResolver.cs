using System;
using System.Collections.Generic;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Interfaces;

namespace HowToGet.CurrencyRates
{
	public class CurrencyRateResolver: ICurrencyRateResolver
	{
		private readonly ICurrencyRatesRepository _repository;
		private readonly ICurrencyRepository _currencyRepository;
		private static readonly Dictionary<int, CurrencyRate> Cache = new Dictionary<int, CurrencyRate>();
		private static int _usdCurrencyId = int.MinValue;

		private static readonly object SyncObj = new object();

		public CurrencyRateResolver(ICurrencyRatesRepository repository, ICurrencyRepository currencyRepository)
		{
			_repository = repository;
			_currencyRepository = currencyRepository;
		}

		public CurrencyRate GetRate(int currencyFromId, int currencyToId)
		{
			lock (SyncObj)
			{
				if (Cache.ContainsKey(currencyFromId))
				{
					var rate = Cache[currencyFromId];
					if (rate.Updated.Date >= DateTime.UtcNow.Date)
						return Cache[currencyFromId];
				}
				return GetFromDb(currencyFromId, currencyToId);
			}
		}

		public CurrencyRate GetRateToUsd(int currencyId)
		{
			if(_usdCurrencyId == int.MinValue)
				_usdCurrencyId = _currencyRepository.GetByCode("USD").Id;

			return GetRate(_usdCurrencyId, currencyId);
		}

		private CurrencyRate GetFromDb(int currencyFromId, int currencyToId)
		{
			var newRate = _repository.GetRate(currencyFromId, currencyToId);
			if (newRate != null)
			{
				Cache[currencyFromId] = newRate;
				return newRate;
			}
			return null;
		}
	}
}