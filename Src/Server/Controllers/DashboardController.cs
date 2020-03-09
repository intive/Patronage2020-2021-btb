using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTB.Application.Dashboard.Queries.GetTopListQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BTB.Domain.Entities;

namespace BTB.Server.Controllers
{
    public class DashboardController : BaseController
    {
        /// <summary>
        /// Get list of 10 BTC pairs sorted descending by price
        /// </summary>
        /// <returns>List of <see cref="BinanceSymbolPrice"/></returns>
        /// <response code="200">If list has been acquired successfully.</response>
        /// <response code="500">If error occur during request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetTopListQuery()));
        }
    }
}
