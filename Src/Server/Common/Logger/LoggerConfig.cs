using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common.Logger
{
    public class LoggerConfig
    {
        public bool StackTrace { get; set; }
        public string LogDateFormat { get; set; }
    }
}
