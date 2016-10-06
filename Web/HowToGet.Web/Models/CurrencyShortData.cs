using HowToGet.Models.Dictionaries;

namespace HowToGet.Web.Models
{
	public class CurrencyShortData
	{
		public CurrencyShortData()
		{
			
		}

		public CurrencyShortData(Currency currency)
		{
			Id = currency.Id;
			CurrencyCode = currency.CurrencyCode;
			CurrencyName = currency.CurrencyName;
		}

		public int Id { get; set; }

		public string CurrencyCode { get; set; }

		public string CurrencyName { get; set; } 
	}
}