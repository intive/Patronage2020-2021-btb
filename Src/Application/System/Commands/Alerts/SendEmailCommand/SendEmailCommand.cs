using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Commands.Alerts.SendEmailCommand
{
    public class SendEmailCommand : IRequest
    {
        public string To { get; set; }
        public string EmailTitle { get; set; }
        public string EmailContent { get; set; }
    }
}
