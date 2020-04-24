using BTB.Application.Logs.Common;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BTB.Application.Logs.Commands
{
    public class ClearLogsFromDBCommand : LogRequestBase, IRequest
    {
    }
}
