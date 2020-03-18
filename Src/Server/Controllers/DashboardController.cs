using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BTB.Application.Dashboard.Queries.GetListQuery;
using BTB.Application.Dashboard.Queries.GetTopListQuery;
using BTB.Client.Pages.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BTB.Domain.Entities;
using BTB.Server.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BTB.Server.Controllers
{
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Get list of pairs with pagination!
        /// </summary>
        /// <returns>List of <see cref="BinanceSimpleElement"/></returns>
        /// <response code="200">If list has been acquired successfully.</response>
        /// <response code="500">If error occur during request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PaginationDto pagination, [FromQuery] string name, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetListQuery { Name = name }, cancellationToken);

            var queryable = result.AsQueryable();
            HttpContext.InsertPaginationParameterInResponseHeader(queryable, (int)pagination.Quantity);

            return Ok(queryable.Paginate(pagination).ToList());
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
