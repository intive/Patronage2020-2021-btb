using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Exceptions
{
    public class ValidationExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                context.Result = new ObjectResult(new { ErrorMessage = exception.Message })
                {
                    StatusCode = 400
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
