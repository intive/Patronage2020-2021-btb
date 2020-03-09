using System;
using System.Net;

namespace BTB.Application.Common.Exceptions
{
    public class BadRequestException : HttpResponseException
    {
        public BadRequestException()
            : base(HttpStatusCode.BadRequest, null)
        {
        }

        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, new { ErrorMessage = message })
        {
        }

        public BadRequestException(object value)
            : base(HttpStatusCode.BadRequest, value)
        {
        }
    }
}
