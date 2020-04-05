using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.Alerts.Queries.GetAllAlertsQuery
{
    public class GetAllAlertsQuery : IRequest<PaginatedResult<AlertVO>>
    {
        public PaginationDto Pagination { get; set; }
    }
}
