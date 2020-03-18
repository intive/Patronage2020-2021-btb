using System.Linq;
using BTB.Domain.Common.Pagination;

namespace BTB.Domain.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * (int)pagination.Quantity)
                .Take((int)pagination.Quantity);
        }
    }
}
