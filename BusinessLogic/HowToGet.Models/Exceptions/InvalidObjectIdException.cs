using System;

namespace HowToGet.Models.Exceptions
{
    public class InvalidObjectIdException: Exception
    {
		public InvalidObjectIdException(string message)
            :base(message)
        {
            
        }
    }
}