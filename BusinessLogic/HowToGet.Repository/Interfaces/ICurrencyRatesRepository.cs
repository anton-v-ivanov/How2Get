using System;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Repository.Interfaces
{
	public interface ICurrencyRatesRepository
	{
		DateTime GetLastUpdated();
		
		void SaveCurrencyRate(CurrencyRate rate);
		
		CurrencyRate GetRate(int currencyFromId, int currencyToId);
	}
}