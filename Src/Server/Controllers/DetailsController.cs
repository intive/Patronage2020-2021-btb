﻿using BTB.Application.Details.Queries.GetPriceHistory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Binance.Net.Objects;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace BTB.Server.Controllers
{
    public class DetailsController : BaseController
    {
        /// <summary>
        ///     Gets 10 last prices at which candlesticks for chosen symbol closed
        /// </summary>
        /// <remarks>
        ///     Sample request:
        /// 
        ///         GET /details/{symbol}?interval={interval}
        ///         {
        ///             "symbol": BTCNGN,
        ///             "interval": "OneMinute"
        ///         }
        ///         
        /// </remarks>
        /// <param name="symbol">The symbol to get the prices for</param>
        /// <param name="interval">The candlestick timespan</param>  
        /// <returns>Collection of 10 pairs of: close time of the candlestick and the price at which the candlestick closed </returns>
        /// <response code="200">When successful.</response>
        /// <response code="400">If symbol is not in correct format or is unknown</response>
        [HttpGet]
        [Route("{symbol}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPriceHistory([FromRoute] string symbol, [FromQuery] KlineInterval interval, CancellationToken cancellationToken)
        {
            var priceHistory = await Mediator.Send(new GetPriceHistoryQuery { Symbol = symbol, Interval = interval }, cancellationToken);
            return Ok(priceHistory);
        }
    }
}