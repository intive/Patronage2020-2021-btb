using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTB.Application.Common.Hubs;
using BTB.Application.Common.Interfaces;
using BTB.Application.System.Commands.SendEmailCommand;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BTB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {

        private readonly IHubContext<NotificationHub> _hubcontext;
        private readonly ICurrentUserIdentityService _currentUserIdentity;

        public NotificationController(IHubContext<NotificationHub> hubcontext, ICurrentUserIdentityService currentUserIdentity)
        {
            _hubcontext = hubcontext;
            _currentUserIdentity = currentUserIdentity;
        }

        [HttpPost]
        public async Task PostInBrowser()
        {
            await _hubcontext.Clients.User(_currentUserIdentity.UserId)
                   .SendAsync("inbrowser","Test Notification");
        }
    }
}
