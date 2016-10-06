using System;

namespace HowToGet.Models.Exceptions
{
	public class InternalErrorException : Exception
	{
		public InternalErrorException(string message)
			:base(message)
		{
			
		}
	}
}