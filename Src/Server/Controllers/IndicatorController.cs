using BTB.Application.Indicator.Commands.CalculateRSI;
using BTB.Application.Indicator.Commands.CalculateSMA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTB.Server.Controllers
{
    public class IndicatorController : BaseController
    {
        /// <summary>
        /// Calculates Relative Strength Index indicator and returns result. 
        /// </summary>
        /// <param name="command">Data required to calculate RSI: collection of close prices and chosen RSI Timeframe.</param>
        /// <returns>Collection of calculated RSI indicators.</returns>
        [HttpPost("RSI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CalculateRSI([FromBody] CalculateRSICommand command)
        {
            var indicators = await Mediator.Send(command ?? new CalculateRSICommand()); 
            return Ok(indicators);
        }

        /// <summary>
        /// Calculates Small Moving Average indicator and returns result. 
        /// </summary>
        /// <param name="command">Data required to calculate SMA: collection of prices and chosen timeperiod.</param>
        /// <returns>Collection of calculated SMA indicators.</returns>
        [HttpPost("SMA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CalculateSMA([FromBody] CalculateSMACommand command)
        {
            var indicators = await Mediator.Send(command ?? new CalculateSMACommand());
            return Ok(indicators);
        }

    }
}
