using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.UserManagement.Queries.GetUserListQuery
{
    public class GetUserListQuery : IRequest<PaginatedResult<ApplicationUserVO>>
    {
        public PaginationDto Pagination { get; set; }
    }
}
