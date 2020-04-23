using BTB.Application.Details.Queries.GetPriceHistory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Binance.Net.Objects;
using Microsoft.AspNetCore.Http;
using System.Threading;
using BTB.Domain.Common.Pagination;
using BTB.Domain.Common;
using BTB.Domain.Extensions;
using BTB.Domain.ValueObjects;
using BTB.Application.Common.Models;

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
        /// <param name="pagination">Data required to perform pagination</param>
        /// <param name="dataSource">Source of klines, either database or API</param>
        /// <param name="extraAmount">Additional amount of klines, especially useful for indicators view</param>
        /// <returns>Collection of klines </returns>
        /// <response code="200">When successful.</response>
        /// <response code="400">If symbol is not in correct format or is unknown</response>
        [HttpGet]
        [Route("{symbol}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPriceHistory([FromRoute] string symbol, [FromQuery] KlineInterval interval, [FromQuery] PaginationDto pagination, [FromQuery] DetailsDataSource dataSource, [FromQuery] int extraAmount, CancellationToken cancellationToken)
        {
            PaginatedResult<KlineVO> paginatedResult = await Mediator.Send(new GetPriceHistoryQuery { PairName = symbol, KlineType = interval, Pagination = pagination, DataSource = dataSource, ExtraAmount = extraAmount }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(paginatedResult.AllRecordsCount, paginatedResult.RecordsPerPage);
            return Ok(paginatedResult.Result);
        }
    }
}