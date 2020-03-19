using BTB.Domain.Common.Pagination;
using System.Linq;

namespace BTB.Domain.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * (int)pagination.Quantity)
                .Take((int)pagination.Quantity);
        }
    }
}
