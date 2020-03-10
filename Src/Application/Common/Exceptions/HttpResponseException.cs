using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BTB.Application.Common.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Value { get; set; }

        public HttpResponseException(HttpStatusCode statusCode, object value = null)
        {
            StatusCode = statusCode;
            Value = value;
        }
    }
}
