using System;
using HowToGet.Models.Dictionaries;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface ICurrencyRatesProvider
	{
		DateTime GetLastUpdated();
		
		void SaveCurrencyRate(CurrencyRate rate);
		
		CurrencyRate GetRate(int currencyFromId, int currencyToId);
	}
}