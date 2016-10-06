using System.Configuration;

namespace HowToGet.RouteEngine.Rankers.Configuration
{
	public class PriorityElementCollection : ConfigurationElementCollection 
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new PriorityElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((PriorityElement)element).Name;
		}
	}
}