﻿using BTB.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTB.Application.Common.Hubs
{
    public class NotificationHub : Hub, INotificationHub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAsync(string method, string message)
        {
            if (method == "inbrowser")
            {
                await SendAsync(method, message);
            }
        }
    }
}
