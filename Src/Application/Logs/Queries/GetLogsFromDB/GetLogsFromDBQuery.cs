using BTB.Application.Logs.Common;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BTB.Application.Logs.Queries.GetLogsFromDB
{
    public class GetLogsFromDBQuery : LogRequestBase, IRequest<object[]>
    {
        public int Amount { get; set; }
        public int StartPosition { get; set; }
    }
}
