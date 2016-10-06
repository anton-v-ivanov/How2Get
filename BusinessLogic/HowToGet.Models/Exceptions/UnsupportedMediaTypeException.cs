using System;

namespace HowToGet.Models.Exceptions
{
	public class UnsupportedMediaTypeException: Exception
	{
		public UnsupportedMediaTypeException(string message)
			:base(message)
		{
			
		}
		 
	}
}