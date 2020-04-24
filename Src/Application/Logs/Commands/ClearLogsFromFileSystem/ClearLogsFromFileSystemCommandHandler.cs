using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.FileProviders;
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
    public class ClearLogsFromFileSystemCommandHandler : IRequestHandler<ClearLogsFromFileSystemCommand, int>
    {
        private readonly IBTBDbContext _context;
        private readonly ILogFileService _logFileService;
        private readonly ILogger _logger;

        public ClearLogsFromFileSystemCommandHandler(IBTBDbContext context, ILogFileService logFileCommander, ILogger<ClearLogsFromFileSystemCommandHandler> logger)
        {
            _context = context;
            _logFileService = logFileCommander;
            _logger = logger;
        }

        public async Task<int> Handle(ClearLogsFromFileSystemCommand request, CancellationToken cancellationToken)
        {
            var dateTime = DateTime.ParseExact(request.LogDate, _logFileService.LogDateFormat, CultureInfo.InvariantCulture);

            var filePaths = _logFileService.GetLogFilePaths(dateTime, (LogLevel)request.LogLevel, request.NameContains);
            int deleted = 0;

            foreach (string path in filePaths)
            {
                if (_logFileService.RemoveFileFromServer(path))
                {
                    deleted++;
                }
            }

            if (deleted == 0)
            {
                var e = new NotFoundException($"No {request.LogLevel} logs containing '{request.NameContains}' from date {dateTime.Date} were found");
                _logger.LogError("Error during log operations.", new object[] { e });
                throw e;
            }

            return deleted;
        }
    }
}
