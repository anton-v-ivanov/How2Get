using System.Configuration;

namespace HowToGet.Bonuses.Configuration
{
	public class BonusElementCollection : ConfigurationElementCollection 
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new BonusElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((BonusElement)element).Name;
		}
	}
}