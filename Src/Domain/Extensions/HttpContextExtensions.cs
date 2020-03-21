using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace BTB.Domain.Extensions
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationParameterInResponseHeader(this HttpContext httpContext,
            int allRecordsCount, int recordsPerPage)
        {
            double pagesQuantity = Math.Ceiling((double)allRecordsCount / recordsPerPage);
            httpContext.Response.Headers.Add("quantity", $"{pagesQuantity}");
        }
    }
}
