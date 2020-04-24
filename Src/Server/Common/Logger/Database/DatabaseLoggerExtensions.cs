using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger.Database
{
    public static class DatabaseLoggerExtensions
    {
        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder loggerBuilder, DatabaseLoggerConfig config)
        {
            loggerBuilder.AddProvider(new DatabaseLoggerProvider(config));
            return loggerBuilder;
        }

        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder loggerBuilder)
        {
            var config = new DatabaseLoggerConfig();
            return loggerBuilder.AddDatabaseLogger(config);
        }

        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder loggerBuilder, Action<DatabaseLoggerConfig> configure)
        {
            var config = new DatabaseLoggerConfig();
            configure(config);
            return loggerBuilder.AddDatabaseLogger(config);
        }
    }
}
