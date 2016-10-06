using System;

namespace HowToGet.Security.Helpers
{
	public class SecurityHelper
	{
		public static T GetConfigValue<T>(string configValue, T defaultValue)
		{
			if (String.IsNullOrEmpty(configValue))
				return defaultValue;

			return ((T)Convert.ChangeType(configValue, typeof(T)));
		}
	}
}
