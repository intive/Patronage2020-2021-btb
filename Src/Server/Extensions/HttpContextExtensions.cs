using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BTB.Server.Extensions
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
