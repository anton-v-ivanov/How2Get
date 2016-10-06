using System;

namespace HowToGet.Models.Exceptions
{
    public class ObjectNotFoundException: Exception
    {
        public ObjectNotFoundException(string message)
            :base(message)
        {
            
        }
    }
}