using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBrowserNotificationHub
    {
        Task SendToUserAsync(string userId, string message);
        Task SendToAllUsersAsync(string message);
    }
}
