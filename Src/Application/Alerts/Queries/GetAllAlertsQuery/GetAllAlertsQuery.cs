using BTB.Application.Alerts.Common;
using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using MediatR;

namespace BTB.Application.Alerts.Queries.GetAllAlertsQuery
{
    public class GetAllAlertsQuery : IRequest<PaginatedResult<AlertVm>>
    {
        public PaginationDto Pagination { get; set; }
    }
}
