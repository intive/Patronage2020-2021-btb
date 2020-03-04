using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Interfaces
{
    public interface IEmailService
    {
        public void Send(string to, string title, string message);
    }
}
