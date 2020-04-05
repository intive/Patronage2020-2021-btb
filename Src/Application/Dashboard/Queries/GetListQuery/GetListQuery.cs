using MediatR;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using BTB.Application.Common.Models;

namespace BTB.Application.Dashboard.Queries.GetListQuery
{
    public class GetListQuery : IRequest<PaginatedResult<DashboardPairVO>>
    {
        public PaginationDto Pagination { get; set; }
        public string Name { get; set; }
    }
}
