using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.FavoriteSymbolPairs.Queries.GetAllFavoriteSymbolPairs
{
    public class GetAllFavoriteSymbolPairsQuery : IRequest<PaginatedResult<DashboardPairVO>>
    {
        public PaginationDto Pagination { get; set; }
        public string Name { get; set; }
    }
}
