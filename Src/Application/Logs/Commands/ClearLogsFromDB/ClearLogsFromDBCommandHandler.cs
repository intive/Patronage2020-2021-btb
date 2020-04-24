using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Logs.Commands
{
    public class ClearLogsFromDBCommandHandler : IRequestHandler<ClearLogsFromDBCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly ILogFileService _logFileService;
        private readonly ILogger _logger;

        public ClearLogsFromDBCommandHandler(IBTBDbContext context, ILogFileService logFileService, ILogger<ClearLogsFromDBCommand> logger)
        {
            _context = context;
            _logFileService = logFileService;
            _logger = logger;
        }

        public async Task<Unit> Handle(ClearLogsFromDBCommand request, CancellationToken cancellationToken)
        {
            var dateTime = DateTime.ParseExact(request.LogDate, _logFileService.LogDateFormat, CultureInfo.InvariantCulture);

            var response = _context.Logs.Where(l =>
                    l.TimeStampUtc.Date == dateTime.Date &&
                    l.Level == request.LogLevel &&
                    l.Category.Contains(request.NameContains)
                ).ToList();

            if (response.Count == 0)
            {
                throw new NotFoundException($"No {request.LogLevel} logs containing '{request.NameContains}' from date {dateTime.Date} were found");
            }

            _context.Logs.RemoveRange(response);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}
