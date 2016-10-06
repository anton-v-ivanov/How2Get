using System;

namespace HowToGet.Models.Exceptions
{
	public class SecurityException : Exception
	{
		public SecurityException(string message)
			:base(message)
		{
			
		}
	}
}