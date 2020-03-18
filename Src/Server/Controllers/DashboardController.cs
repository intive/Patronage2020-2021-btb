using System;
using System.Linq;
using System.Threading;
using BTB.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using BTB.Domain.Common.Pagination;
using BTB.Application.Dashboard.Queries.GetListQuery;
using BTB.Application.Dashboard.Queries.GetTopListQuery;

namespace BTB.Server.Controllers
{
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Get list of pairs with pagination!
        /// </summary>
        /// <returns>List of <see cref="BinanceSimpleElement"/></returns>
        /// <response code="200">If list has been acquired successfully.</response>
        /// <response code="400">If name is not found or page and quantity is out of range. </response>
        /// <response code="500">If error occur during request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PaginationDto pagination, [FromQuery] string name, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetListQuery { Pagination = pagination, Name = name }, cancellationToken));
        }

        /// <summary>
        /// Get list of 10 BTC pairs sorted descending by price
        /// </summary>
        /// <returns>List of <see cref="BinanceSimpleElement"/></returns>
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
