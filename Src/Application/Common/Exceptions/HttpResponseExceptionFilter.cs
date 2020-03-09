using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Exceptions
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = (int)exception.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
