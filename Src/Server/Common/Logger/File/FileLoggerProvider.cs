using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using BTB.Server.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger
{
    public class FileLoggerProvider : LoggerProviderBase
    {
        private readonly FileLoggerConfig _config;
        private readonly ILogFileService _logCommander;

        public FileLoggerProvider(FileLoggerConfig config) : base(config)
        {
            _config = config;
            _logCommander = new LogFileSystemService(config);
        }


        public override bool IsEnabled(LogLevel logLevel)
        {
            return
                (logLevel != LogLevel.None) &&
                ((int)logLevel >= (int)_config.MinLogLevel);
        }

        public override Task WriteLogAsync(LogEntryVO log)
        {
            if (IsEnabled(log.Level))
            {
                _logCommander.SaveLogToFile(log);
            }

            return Task.CompletedTask;
        }
    }
}
