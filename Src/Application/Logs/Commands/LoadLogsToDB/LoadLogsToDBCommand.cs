using BTB.Domain.ValueObjects;
using MediatR;
using System.Collections.Generic;

namespace BTB.Application.Logs.Commands
{
    public class LoadLogsToDBCommand : IRequest
    {
        public List<LogEntryVO> LogList { get; set; }
    }
}
