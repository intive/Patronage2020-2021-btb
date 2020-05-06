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

        public NotificationController(IBrowserNotificationHub hub, IUserAccessor userAccessor)
        {
            _hub = hub;
            _userAccessor = userAccessor;
        }

        [HttpPost]
        public async Task PostInBrowser()
        {
            await _hub.SendToUserAsync(_userAccessor.GetCurrentUserId(), "xDDDDDDDDDDD");
        }
    }
}
