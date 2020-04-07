using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Commands.SendEmailCommand
{
    public class SendEmailCommand : IRequest
    {
        public string To { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
