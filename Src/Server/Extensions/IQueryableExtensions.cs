using System.Linq;
using BTB.Client.Pages.Dto;

namespace BTB.Server.Extensions
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
