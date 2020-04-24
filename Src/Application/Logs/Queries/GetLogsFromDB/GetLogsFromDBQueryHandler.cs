using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Logs.Queries.GetLogsFromDB
{
    public class GetLogsFromDBQueryHandler : IRequestHandler<GetLogsFromDBQuery, object[]>
    {
        private readonly IBTBDbContext _context;
        private readonly ILogFileService _logFileService;

        public GetLogsFromDBQueryHandler(IBTBDbContext context, ILogFileService logFileService)
        {
            _context = context;
            _logFileService = logFileService;
        }

        public async Task<object[]> Handle(GetLogsFromDBQuery request, CancellationToken cancellationToken)
        {
            Func<LogEntry, bool> basicLogs = log =>
            {
                return
                (string.IsNullOrEmpty(request.LogDate) ? true : log.TimeStampUtc.Date.ToString(_logFileService.LogDateFormat) == request.LogDate)
                    &&
                (request.LogLevel == default ? true : log.Level == request.LogLevel)
                    &&
                (string.IsNullOrEmpty(request.NameContains) ? true : log.Category.Contains(request.NameContains));
            };

            int currentPos = 0;
            int amount = 0;
            Func<LogEntry, bool> cuttedLogs = log =>
            {
                currentPos++;
                return ((currentPos >= request.StartPosition) && (currentPos < request.StartPosition + request.Amount));
            };

            var response = _context.Logs.Where(basicLogs).OrderByDescending(d => d.TimeStampUtc).ToList();
            int quantity = response.Count;

            var cuttedResponse = response.Where(cuttedLogs).ToList();
            var valueObjects = cuttedResponse.Select(d => new LogEntryVO(d)).ToList(); 
            
            return new object[] { quantity, valueObjects };
        }
    }
}
