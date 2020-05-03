using System;

namespace BTB.Application.Common.Exceptions.GamblePointManager
{
    public class DoesNotExistException : Exception
    {
        public DoesNotExistException()
        {

        }

        public DoesNotExistException(string message)
            :base(message)
        { 
        }
    }
}
