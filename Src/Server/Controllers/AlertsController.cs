using System.Threading.Tasks;
using BTB.Application.Alerts.Commands.CreateAlert;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    public class AlertsController : BaseController
    {
        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateAlertCommand command)
        {
            await Mediator.Send(command);

            return Ok();
        }
    }
}