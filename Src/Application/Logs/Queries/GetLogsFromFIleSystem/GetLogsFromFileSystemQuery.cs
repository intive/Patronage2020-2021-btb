using BTB.Application.Logs.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BTB.Application.Logs.Queries.GetLogsFromFileSystem
{
    public class GetLogsFromFileSystemQuery : LogRequestBase, IRequest<List<byte>>
    {
        public int? Limit { get; set; }
    }
}
