using BTB.Application.Common.Interfaces;
using BTB.Application.Logs.Commands;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger.Database
{
    public class DatabaseLoggerProvider : LoggerProviderBase
    {
        private DatabaseLoggerConfig _config; 

        private static List<LogEntryVO> _unSavedLogList;
        private static List<LogEntryVO> UnSavedLogList
        {
            get
            {
                var copiedList = _unSavedLogList.GetRange(0, _unSavedLogList.Count);
                _unSavedLogList = new List<LogEntryVO>();
                return copiedList;
            }
        }

        static DatabaseLoggerProvider()
        {
            List<Exception> configureServicesEx = Startup.ConfigureServicesExceptions.Values.ToList();
            List<Exception> servicesEx = Startup.ConfigureExceptions.Values.ToList();
            List<Exception> programEx = Program.ProgramExceptions;

            IEnumerable<Exception> exceptions = configureServicesEx.Concat(servicesEx).Concat(programEx);

            if (_unSavedLogList == null)
            {
                _unSavedLogList = new List<LogEntryVO>();
            }

            _unSavedLogList.AddRange(exceptions.Select(e => new LogEntryVO(e, true)
            {
                Category = nameof(Startup),
                Level = LogLevel.Error,
                Text = e.Message ?? e.ToString()
            }).ToList());
        }

        public DatabaseLoggerProvider(DatabaseLoggerConfig options) : base(options)
        {
            _config = options;
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            return
                (logLevel != LogLevel.None) &&
                ((int)logLevel >= (int)_config.MinLogLevel);
        }

        public async override Task WriteLogAsync(LogEntryVO log)
        {
            _unSavedLogList.Add(log);

            if (log.Level >= LogLevel.Error)
            {
                await LogToDatabase();
            }
        }

        public static Task ForceLog()
        {
            return LogToDatabase();
        }

        private static async Task LogToDatabase()
        {
            var serviceProvider = Program.ServiceProvider;

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IBTBDbContext>();
                var handlerLogger = scope.ServiceProvider.GetRequiredService<ILogger<LoadLogsToDBCommandHandler>>();

                var logCommandHandler = new LoadLogsToDBCommandHandler(context, handlerLogger);
                var logList = UnSavedLogList;
                await logCommandHandler.Handle(new LoadLogsToDBCommand() { LogList = logList }, CancellationToken.None);
            }
        }
    }
}
