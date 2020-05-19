﻿using BTB.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BTB.Application.Common.Hubs
{
   
    public class NotificationHub : Hub
    {
        public static readonly ConcurrentDictionary<string, List<string>> UserConnections = new ConcurrentDictionary<string, List<string>>();

        public override Task OnConnectedAsync()
        {
            AddConnectionToUser();
            return base.OnConnectedAsync();
        }

        private void AddConnectionToUser()
        {
          string userId = Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;            

          if (!string.IsNullOrEmpty(userId))
            {
                var connections = UserConnections.GetValueOrDefault(userId);

                if (connections == null)
                {
                    connections = new List<string>();
                }

                connections.Add(Context.ConnectionId);
                UserConnections[userId] = connections;
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            RemoveConnectionFromUser();
            return base.OnDisconnectedAsync(exception);
        }

        private void RemoveConnectionFromUser()
        {
            string userId = Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var connections = UserConnections.GetValueOrDefault(userId);

                if (connections == null)
                {
                    return;
                }

                connections.Remove(Context.ConnectionId);
                UserConnections[userId] = connections;
            }
        }
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
            List<string> connections = NotificationHub.UserConnections.GetValueOrDefault(userId);

            if (connections == null)
            {
                return;
            }

            var lastConnection = connections.LastOrDefault();

            if (!string.IsNullOrEmpty(lastConnection))
            {
                await _hubcontext.Clients.Clients(lastConnection).SendAsync("inbrowser", message);
            }
        }

        public async Task SendToAllUsersAsync(string message)
        {
            await _hubcontext.Clients.All.SendAsync("inbrowser", message);
        }
    }
}