using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTB.Application.System.Commands.SendEmailNotificationsCommand;
using BTB.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        [HttpPost]
        public async Task PostInBrowser()
        {
            await Mediator.Send(new SendNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin });
        }
    }
}
