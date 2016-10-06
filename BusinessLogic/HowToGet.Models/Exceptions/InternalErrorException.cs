using System;

namespace HowToGet.Models.Exceptions
{
	public class ValidationException : Exception
	{
		public ValidationException(string message)
			:base(message)
		{
			
		}
	}
}