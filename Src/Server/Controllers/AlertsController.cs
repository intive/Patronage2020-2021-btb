﻿using System.Threading;
using System.Threading.Tasks;
using BTB.Application.Alerts.Commands.CreateAlert;
using BTB.Application.Alerts.Commands.DeleteAlertCommand;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.Alerts.Common;
using BTB.Application.Alerts.Queries.GetAllAlertsQuery;
using BTB.Domain.Common.Pagination;
using BTB.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class AlertsController : BaseController
    {
        /// <summary>
        /// Returns all alerts belonging to the currently logged in user.
        /// </summary>
        /// <param name="pagination">Pagination DTO.</param>
        /// <returns>A paginated list of alert DTOs. <see cref="AlertVm" /></returns>
        /// <response code="200">When successful.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination, CancellationToken cancellationToken)
        {
            var paginatedResult = await Mediator.Send(new GetAllAlertsQuery() { Pagination = pagination }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(paginatedResult.AllRecorsCount, paginatedResult.RecordsPerPage);
            return Ok(paginatedResult.Result);
        }

        /// <summary>
        /// Creates an alert for the currently logged in user.
        /// </summary>
        /// <param name="command"> From-body data to create an alert. </param>
        /// <response code="201">When alert is created successfully.</response>
        /// <response code="400">When validation error occurs.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAlertCommand command, CancellationToken cancellationToken)
        {
            return CreatedAtAction(nameof(Create), await Mediator.Send(command, cancellationToken));
        }

        /// <summary>
        /// Updates an alert specified by id. The alert must belong to the logged in user.
        /// </summary>
        /// <param name="id">Id of the alert.</param>
        /// <response code="200">When successful.</response>
        /// <response code="400">When validation error occurs.</response>
        /// <response code="404">When alert with given id does not exist or does not belong to the user.</response>
        [Route("{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAlertCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await Mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Deletes an alert specified by id. The alert must belong to the logged in user.
        /// </summary>
        /// <param name="id">Id of the alert.</param>
        /// <response code="200">When successful.</response>
        /// <response code="404">When alert with given id does not exist or does not belong to the user.</response>
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteAlertCommand() { Id = id }, cancellationToken);
            return Ok();
        }
    }
}