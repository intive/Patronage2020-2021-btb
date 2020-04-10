using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface INotificationHub
    {
        Task SendAsync(string method, string message);
    }
}