using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.UserManagement.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<PaginatedResult<ApplicationUserVO>>
    {
        public PaginationDto Pagination { get; set; }
    }
}
