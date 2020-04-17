using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BTB.Application.Common.Exceptions
{
    public class ServiceUnavailableException : HttpResponseException
    {
        public ServiceUnavailableException() : base(HttpStatusCode.ServiceUnavailable, null)
        {
        }

        public ServiceUnavailableException(string message) : base(HttpStatusCode.ServiceUnavailable, new { ErrorMessage = message })
        {
        }

        public ServiceUnavailableException(object value) : base(HttpStatusCode.ServiceUnavailable, value)
        {
        }
    }
}
