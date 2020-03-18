using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace BTB.Domain.Extensions
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationParameterInResponseHeader<T>(this HttpContext httpContext, 
            IQueryable<T> queryable, int recordsPerPage)
        {
            double count = queryable.Count();
            double pagesQuantity = Math.Ceiling(count / recordsPerPage);
            httpContext.Response.Headers.Add("quantity", $"{pagesQuantity}");
        }
    }
}
