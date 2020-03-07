using System.Threading.Tasks;
using BTB.Application.Alerts.Commands.CreateAlert;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    public class AlertsController : BaseController
    {

        /// <summary>
        ///     Sends request to save new alert in database.
        /// </summary>
        /// <param name="command">
        ///     From-body data to create an alert.
        /// </param>
        /// <returns>
        ///     Ok()
        /// </returns>
        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateAlertCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}