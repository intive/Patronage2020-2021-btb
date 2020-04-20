using BTB.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Commands.SendNotificationsCommand
{
    public class SendNotificationsCommand : IRequest
    {
        public TimestampInterval KlineInterval { get; set; } = TimestampInterval.FiveMin;
    }
}
