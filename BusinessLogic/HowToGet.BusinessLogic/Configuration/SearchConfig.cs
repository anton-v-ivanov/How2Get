using System.Configuration;

namespace HowToGet.BusinessLogic.Configuration
{
	public class SearchConfig : ConfigurationSection
	{
		public static SearchConfig Instance { get; private set; }

		static SearchConfig()
		{
			if(Instance == null)
				Instance = (SearchConfig)ConfigurationManager.GetSection("search");
		}

		private static int _citySearchResultCount;
		[ConfigurationProperty("city-result-count", IsRequired = true)]
		public int CitySearchResultCount
		{
			get
			{
				if (_citySearchResultCount == 0)
					_citySearchResultCount = (int)this["city-result-count"];
				return _citySearchResultCount;
			}
		}

		private static int _routeCacheTimeMinutes;
		[ConfigurationProperty("route-cache-time-minutes", IsRequired = true)]
		public int RouteCacheTimeMinutes
		{
			get
			{
				if (_routeCacheTimeMinutes == 0)
					_routeCacheTimeMinutes = (int)this["route-cache-time-minutes"];
				return _routeCacheTimeMinutes;
			}
		}

		private static int _maxResultCount;
		[ConfigurationProperty("max-result-count", IsRequired = true)]
		public int MaxResultCount
		{
			get
			{
				if (_maxResultCount == 0)
					_maxResultCount = (int)this["max-result-count"];
				return _maxResultCount;
			}
		}

		private static int _maxTransferCount;
		[ConfigurationProperty("max-transfer-count", IsRequired = true)]
		public int MaxTransferCount
		{
			get
			{
				if (_maxTransferCount == 0)
					_maxTransferCount = (int)this["max-transfer-count"];
				return _maxTransferCount;
			}
		}
	}
}