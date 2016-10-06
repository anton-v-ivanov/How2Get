using HowToGet.Services.Quartz.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HowToGet.CurrencyRates.Downloader.Tests
{
	[TestClass]
	public class DownloaderTests
    {
		[TestMethod]
		public void DownloadRatesTest()
		{
			const string url = "http://finance.yahoo.com/webservice/v1/symbols/allcurrencies/quote";

			var downloader = new CurrencyRatesDownloadJob();
			downloader.DownloadRates(url);
		}

		[TestMethod]
		public void GetCurrenciesWithoutRatesTest()
		{
			var downloader = new CurrencyRatesDownloadJob();
			downloader.GetCurrenciesWithoutRates();
		}
    }
}
