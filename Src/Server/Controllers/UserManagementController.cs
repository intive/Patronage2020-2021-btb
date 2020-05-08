using BTB.Application.UserManagement.Commands.TakeRoleFromUser;
using BTB.Application.UserManagement.Commands.GiveRoleToUser;
using BTB.Application.UserManagement.Queries.GetRoleList;
using BTB.Application.UserManagement.Queries.GetUserList;
using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using BTB.Domain.Extensions;
using BTB.Domain.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace BTB.Server.Controllers
{
    [Authorize(Policy = Policies.IsAdmin)]
    public class UserManagementController : BaseController
    {
        /// <summary>
        /// Gets list of all users.
        /// </summary>
        /// <param name="pagination">Data needed to perform pagination</param>
        /// <returns>List of users</returns>
        [HttpGet("GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserList([FromQuery] PaginationDto pagination, CancellationToken cancellationToken)
        {
            PaginatedResult<ApplicationUserVO> paginatedResult = await Mediator.Send(new GetUserListQuery() { Pagination = pagination }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(paginatedResult.AllRecordsCount, paginatedResult.RecordsPerPage);
            return Ok(paginatedResult.Result);
        }

        /// <summary>
        /// Gets list of all roles.
        /// </summary>
        /// <returns>List of roles.</returns>
        [HttpGet("GetRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRoleList(CancellationToken cancellationToken)
        {
            List<string> roles = await Mediator.Send(new GetRoleListQuery(), cancellationToken);
            return Ok(roles);
        }

        /// <summary>
        /// Allows to give new role to the user.
        /// </summary>
        /// <param name="command">Data needed to perform action</param>
        /// <returns>Roles of the user.</returns>
        [HttpPut("GiveRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GiveRoleToUser([FromBody] GiveRoleToUserCommand command, CancellationToken cancellationToken)
        {
            UserRolesVO usersRoles = await Mediator.Send(command ?? new GiveRoleToUserCommand(), cancellationToken);
            return Ok(usersRoles);
        }

        /// <summary>
        /// Allows to take away one of the user's role.
        /// </summary>
        /// <param name="command">Data needed to perform action</param>
        /// <returns>Roles of the user.</returns>
        [HttpPut("TakeRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> TakeRoleFromUser([FromBody] TakeRoleFromUserCommand command, CancellationToken cancellationToken)
        {
            UserRolesVO usersRoles = await Mediator.Send(command ?? new TakeRoleFromUserCommand(), cancellationToken);
            return Ok(usersRoles);
        }

    }
}