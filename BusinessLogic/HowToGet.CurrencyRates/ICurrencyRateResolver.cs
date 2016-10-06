using HowToGet.Models.Dictionaries;

namespace HowToGet.CurrencyRates
{
	public interface ICurrencyRateResolver
	{
		CurrencyRate GetRate(int currencyFromId, int currencyToId);
		
		CurrencyRate GetRateToUsd(int currencyId);
	}
}