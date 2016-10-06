using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using Common.Logging;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Providers;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;
using Quartz;

namespace HowToGet.Services.Quartz.Jobs
{
	[DisallowConcurrentExecution]
	public class CurrencyRatesDownloadJob : IJob
	{
		private static readonly ILog CurrentLogger = LogManager.GetCurrentClassLogger();
		
		private readonly ICurrencyRatesRepository _ratesRepository;
		private readonly ICurrencyRatesProvider _ratesProvider;
		private readonly ICurrencyRepository _currencyRepository;
		private readonly ICurrencyProvider _currencyProvider;

		public CurrencyRatesDownloadJob()
		{
			_ratesRepository = new CurrencyRatesRepository();
			_ratesProvider = new CurrencyRatesProvider(_ratesRepository);

			_currencyRepository = new CurrencyRepository();
			int euroCurrencyCode = Convert.ToInt32(ConfigurationManager.AppSettings["euro-currency-code"]);
			int usdCurrencyCode = Convert.ToInt32(ConfigurationManager.AppSettings["usd-currency-code"]);
			_currencyProvider = new CurrencyProvider(_currencyRepository, euroCurrencyCode, usdCurrencyCode);
		}

		public void Execute(IJobExecutionContext context)
		{
			CurrentLogger.Info("CurrencyRates download job started");
			UpdateCurrencyRates();
		}

		private void UpdateCurrencyRates()
		{
			var lastUpdated = _ratesProvider.GetLastUpdated();
			if (DateTime.UtcNow.Date <= lastUpdated.Date)
			{
				CurrentLogger.InfoFormat("Last update was {0}. Currency rates are up to date.", lastUpdated);
				return;
			}

			CurrentLogger.InfoFormat("Last update was {0}. Updating rates", lastUpdated);
			string url = ConfigurationManager.AppSettings["currency-rates-url"];
			DownloadRates(url);
		}

		public void DownloadRates(string url)
		{
			try
			{
				string xml = new WebClient().DownloadString(url);

				var ignoredStrings = ConfigurationManager.AppSettings["ignored-currency-codes"].Split(new[] { ',' });
				ParseRates(xml, ignoredStrings.ToList());
			}
			catch (Exception exception)
			{
				CurrentLogger.Error("Unable to download currency rates", exception);
			}

			GetCurrenciesWithoutRates();
		}

		private void ParseRates(string xml, List<string> ignoredCurrencyCodes)
		{
			var format = new NumberFormatInfo { NumberDecimalSeparator = "." };
			var usdCurrency = _currencyProvider.GetByCode("USD");

			var doc = new XmlDocument();
			doc.LoadXml(xml);

			var nodes = doc.SelectNodes("/list/resources/resource");
			if (nodes == null)
				return;


			foreach (XmlNode node in nodes)
			{
				string name = string.Empty;
				string value = string.Empty;
				foreach (XmlNode childNode in node.ChildNodes)
				{
					if (childNode.Attributes != null
						&& childNode.Attributes["name"] != null)
					{
						if (childNode.Attributes["name"].Value.Equals("name", StringComparison.InvariantCultureIgnoreCase))
							name = childNode.InnerText;

						if (childNode.Attributes["name"].Value.Equals("price", StringComparison.InvariantCultureIgnoreCase))
							value = childNode.InnerText;
					}

					if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
						break;
				}
				if (name.ToLowerInvariant().StartsWith("usd/"))
				{
					var currencyCode = name.ToLowerInvariant().Replace("usd/", string.Empty);
					if (ignoredCurrencyCodes.Contains(currencyCode))
						continue;

					var price = Convert.ToDouble(value, format);

					var currency = _currencyProvider.GetByCode(currencyCode);
					if (currency == null)
					{
						CurrentLogger.ErrorFormat("Unable to find currency with code = {0}", currencyCode);
						continue;
					}

					var rate = new CurrencyRate
					{
						CurrencyFromId = usdCurrency.Id,
						CurrencyToId = currency.Id,
						Rate = price,
						Updated = DateTime.UtcNow
					};

					_ratesProvider.SaveCurrencyRate(rate);
					CurrentLogger.InfoFormat("Currency rate saved. Code: {0}, Price: {1}", name, price);
				}
				else
				{
					CurrentLogger.InfoFormat("Not a currency rate: {0}", name);
				}
			}
		}

		public void GetCurrenciesWithoutRates()
		{
			var usdCurrency = _currencyProvider.GetByCode("USD");
			var currencies = _currencyProvider.GetAll();
			foreach (var currency in currencies)
			{
				var rate = _ratesProvider.GetRate(usdCurrency.Id, currency.Id);
				if (rate == null)
					CurrentLogger.ErrorFormat("Currency rate for {0} (Id = {1}) was not found", currency.CurrencyCode, currency.Id);
			}
		}

	}
}