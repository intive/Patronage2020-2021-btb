using BTB.Domain.Common.Pagination;
using System.Collections.Generic;
using System.Linq;

namespace BTB.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> queryable, PaginationDto pagination, int amountExtra = 0)
        {
            return queryable
                .Skip((pagination.Page - 1) * (int)pagination.Quantity)
                .Take((int)pagination.Quantity + amountExtra);
        }
    }
}
