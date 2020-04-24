using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BTB.Application.UserManagement.Queries.GetUserListQuery;
using BTB.Domain.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BTB.Domain.Extensions;
using BTB.Domain.Policies;

namespace BTB.Server.Controllers
{
    // TODO: Summary
    [Authorize(Policy = Policies.IsAdmin)]
    public class UserManagementController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetUserList([FromQuery] PaginationDto pagination, CancellationToken cancellationToken)
        {
            var paginatedResult = await Mediator.Send(new GetUserListQuery() { Pagination = pagination }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(paginatedResult.AllRecordsCount, paginatedResult.RecordsPerPage);
            return Ok(paginatedResult.Result);
        }
    }
}