using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger.Database
{
    public class DatabaseLoggerConfig : LoggerConfig
    {
        public LogLevel MinLogLevel { get; set; }
    }
}
