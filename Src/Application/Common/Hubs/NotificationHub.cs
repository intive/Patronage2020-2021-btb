using BTB.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTB.Application.Common.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

    // TODO: Move to separate files
    public interface IBrowserNotificationHub
    {
        Task SendToUserAsync(string userId, string message);
    }

    public class BrowserNotificationHub : IBrowserNotificationHub
    {
        private IHubContext<NotificationHub> _hubcontext;

        public BrowserNotificationHub(IHubContext<NotificationHub> hubcontext)
        {
            _hubcontext = hubcontext;
        }

        public async Task SendToUserAsync(string userId, string message)
        {
            await _hubcontext.Clients.Client(userId).SendAsync("inbrowser", message);
        }
    }
}
