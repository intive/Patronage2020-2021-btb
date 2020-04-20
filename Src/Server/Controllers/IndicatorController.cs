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
        [HttpPost("RSI")]
        public async Task<ActionResult> CalculateRSI([FromBody] CalculateRSICommand command)
        {
            var indicators = await Mediator.Send(command ?? new CalculateRSICommand()); 
            return Ok(indicators);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SMA")]
        public async Task<ActionResult> CalculateSMA([FromBody] CalculateSMACommand command)
        {
            var indicators = await Mediator.Send(command ?? new CalculateSMACommand());
            return Ok(indicators);
        }

    }
}
