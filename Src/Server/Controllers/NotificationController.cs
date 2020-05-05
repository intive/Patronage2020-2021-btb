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
    public class NotificationController : BaseController
    {
        private readonly IBrowserNotificationHub _hub;
        private readonly IUserAccessor _userAccessor;

        private readonly IHubContext<NotificationHub> _context;

        public NotificationController(IBrowserNotificationHub hub, IUserAccessor userAccessor, IHubContext<NotificationHub> context)
        {
            _hub = hub;
            _userAccessor = userAccessor;
            _context = context;
        }

        [HttpPost]
        public async Task PostInBrowser()
        {
            await _context.Clients.AllExcept(new List<string>() { _userAccessor.GetCurrentUserId() }).SendAsync("inbrowser", "Except Notification"); ;
            await _context.Clients.User(_userAccessor.GetCurrentUserId()).SendAsync("inbrowser", "User Notification");
            await _context.Clients.Client(_userAccessor.GetCurrentUserId()).SendAsync("inbrowser", "Client Notification");
        }
    }
}
