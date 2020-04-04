using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTB.Application.Common.Interfaces;
using BTB.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BTB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubcontext;
        private readonly ICurrentUserIdentityService _currentUserIdentity;

        public NotificationsController(IHubContext<NotificationHub> hubContext, ICurrentUserIdentityService currentUserIdentity)
            => (_hubcontext, _currentUserIdentity) = (hubContext, currentUserIdentity);

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string message)
        {
            await _hubcontext.Clients.All.SendAsync("notifications", $"{message}");
            return Ok("Sent!");
        }
    }
}
