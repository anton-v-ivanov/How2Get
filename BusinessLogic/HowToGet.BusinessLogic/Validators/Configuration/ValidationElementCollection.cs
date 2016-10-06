using System.Configuration;

namespace HowToGet.BusinessLogic.Validators.Configuration
{
	public class ValidationElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ValidationElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ValidationElement)element).Type;
		}
	}
}