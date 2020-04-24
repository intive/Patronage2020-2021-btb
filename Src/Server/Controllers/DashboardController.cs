using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BTB.Domain.Common.Pagination;
using BTB.Application.Dashboard.Queries.GetListQuery;
using BTB.Application.Dashboard.Queries.GetTopListQuery;
using BTB.Domain.Extensions;
using BTB.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Get list of pairs with pagination!
        /// </summary>
        /// <returns>List of <see cref="DashboardPairVO"/></returns>
        /// <response code="200">If list has been acquired successfully.</response>
        /// <response code="400">If name is not found or page and quantity is out of range. </response>
        /// <response code="500">If error occur during request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PaginationDto pagination, [FromQuery] string name, CancellationToken cancellationToken)
        {
            var paginatedResult = await Mediator.Send(new GetListQuery { Pagination = pagination, Name = name }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(paginatedResult.AllRecordsCount, paginatedResult.RecordsPerPage);
            return Ok(paginatedResult.Result);
        }

        /// <summary>
        /// Get list of 10 BTC pairs sorted descending by price
        /// </summary>
        /// <returns>List of <see cref="SimplePriceVO"/></returns>
        /// <response code="200">If list has been acquired successfully.</response>
        /// <response code="500">If error occur during request.</response>
        [HttpGet]
        [Route("top")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTop()
        {
            return Ok(await Mediator.Send(new GetTopListQuery()));
        }
    }
}
