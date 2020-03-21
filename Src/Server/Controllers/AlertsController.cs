using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BTB.Application.Alerts.Commands.CreateAlert;
using BTB.Application.Alerts.Queries.GetAllAlertsQuery;
using BTB.Client.Pages.Dto;
using BTB.Domain.Common.Pagination;
using BTB.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class AlertsController : BaseController
    {
        /// <summary>
        /// Creates an alert for the currently logged in user.
        /// </summary>
        /// <param name="command"> From-body data to create an alert. </param>
        /// <response code="201">When alert is created successfully.</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateAlertCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Create), null);
        }

        /// <summary>
        /// Returns all alerts belonging to the currently logged in user.
        /// </summary>
        /// <param name="pagination">Pagination DTO.</param>
        /// <returns>A list of alert DTOs with pagination. <see cref="AlertDto" /></returns>
        /// <response code="200">When successful.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetAllAlertsQuery() { Pagination = pagination }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(result.AllRecorsCount, result.RecordsPerPage);
            return Ok(result.Result);
        }
    }
}