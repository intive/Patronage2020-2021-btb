using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder loggerBuilder, FileLoggerConfig config)
        {
            loggerBuilder.AddProvider(new FileLoggerProvider(config));
            return loggerBuilder;
        }

        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder loggerBuilder)
        {
            var config = new FileLoggerConfig();
            return loggerBuilder.AddFileLogger(config);
        }

        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder loggerBuilder, Action<FileLoggerConfig> configure)
        {
            var config = new FileLoggerConfig();
            configure(config);
            return loggerBuilder.AddFileLogger(config);
        }
    }
}
