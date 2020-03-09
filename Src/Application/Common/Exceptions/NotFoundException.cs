using System;
using System.Net;

namespace BTB.Application.Common.Exceptions
{
    public class NotFoundException : HttpResponseException
    {
        public NotFoundException()
            : base(HttpStatusCode.NotFound, null)
        {
        }

        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, new { ErrorMessage = message })
        {
        }

        public NotFoundException(object value)
            : base(HttpStatusCode.NotFound, value)
        {
        }
    }
}
