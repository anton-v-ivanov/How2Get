using System;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Interfaces;

namespace HowToGet.BusinessLogic.Providers
{
	public class CurrencyRatesProvider : ICurrencyRatesProvider
	{
		private readonly ICurrencyRatesRepository _repository;

		public CurrencyRatesProvider(ICurrencyRatesRepository repository)
		{
			_repository = repository;
		}

		public DateTime GetLastUpdated()
		{
			return _repository.GetLastUpdated();
		}

		public void SaveCurrencyRate(CurrencyRate rate)
		{
			_repository.SaveCurrencyRate(rate);
		}

		public CurrencyRate GetRate(int currencyFromId, int currencyToId)
		{
			return _repository.GetRate(currencyFromId, currencyToId);
		}
	}
}