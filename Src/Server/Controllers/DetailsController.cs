using BTB.Application.Details.Queries.GetPriceHistory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Binance.Net.Objects;

namespace BTB.Server.Controllers
{
    public class DetailsController : BaseController
    {
        /// <summary>
        ///     Gets last 10 prices at which candlesticks for chosen symbol closed
        /// </summary>
        /// <param name="symbol"></param>
        ///     The symbol to get the prices for
        /// <param name="interval"></param>
        ///     The candlestick timespan
        /// <returns></returns>
        [HttpGet("{symbol}")]
        public async Task<ActionResult> GetPriceHistory([FromRoute] string symbol, [FromQuery] KlineInterval interval)
        {
            var priceHistory = await Mediator.Send(new GetPriceHistoryQuery { Symbol = symbol, Interval = interval });
            return Ok(priceHistory);
        }

    }
}