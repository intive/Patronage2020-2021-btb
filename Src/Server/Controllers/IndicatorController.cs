using BTB.Application.Indicator.Commands.CalculateRSI;
using BTB.Application.Indicator.Commands.CalculateSMA;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTB.Server.Controllers
{
    public class IndicatorController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("rsi")]
        public async Task<ActionResult> CalculateRSI([FromBody] CalculateRSICommand command)
        {
            var indicators = await Mediator.Send(command);
            return Ok(indicators);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("sma")]
        public async Task<ActionResult> CalculateSMA([FromBody] CalculateSMACommand command)
        {
            var indicators = await Mediator.Send(command);
            return Ok(indicators);
        }

    }
}
