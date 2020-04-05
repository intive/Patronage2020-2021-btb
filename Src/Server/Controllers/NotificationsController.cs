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
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubcontext;
        private readonly ICurrentUserIdentityService _currentUserIdentity;

        public NotificationController(IHubContext<NotificationHub> hubContext, ICurrentUserIdentityService currentUserIdentity)
            => (_hubcontext, _currentUserIdentity) = (hubContext, currentUserIdentity);

        [HttpPost]
        public async Task<IActionResult> PostInBrowser([FromQuery] string message)
        {
            await _hubcontext.Clients.All.SendAsync("inbrowser", 
                $"{message} for {_currentUserIdentity.UserId}");

            return Ok($"Sent { message} for { _currentUserIdentity.UserId}");
        }
    }
}
