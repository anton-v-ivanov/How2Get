using System.Configuration;

namespace HowToGet.RouteEngine.Rankers.Configuration
{
	public class PriorityElement : ConfigurationElement
	{
		private const string ByTimeNameElement = "by-time-priority";
		private const string ByPriceNameElement = "by-price-priority";
		private const string DefaultNameElement = "default-priority";
		private const string NameElement = "name";

		[ConfigurationProperty(NameElement, IsKey = true, IsRequired = true)]
		public string Name
		{
			get { return (string)base[NameElement]; }
			set { base[NameElement] = value; }
		}
		
		[ConfigurationProperty(ByTimeNameElement, IsRequired = true)]
		public int SortByTimePriority
		{
			get { return (int)base[ByTimeNameElement]; }
			set { base[ByTimeNameElement] = value; }
		}	 
		
		[ConfigurationProperty(ByPriceNameElement, IsRequired = true)]
		public int SortByPricePriority
		{
			get { return (int)base[ByPriceNameElement]; }
			set { base[ByPriceNameElement] = value; }
		}	 
		
		[ConfigurationProperty(DefaultNameElement, IsRequired = true)]
		public int DefaultPriority
		{
			get { return (int)base[DefaultNameElement]; }
			set { base[DefaultNameElement] = value; }
		}	 
	}
}