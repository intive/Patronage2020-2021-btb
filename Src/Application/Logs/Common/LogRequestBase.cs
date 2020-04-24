using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Logs.Common
{
    public class LogRequestBase
    {
        public string? LogDate { get; set; }
        public LogLevel? LogLevel { get; set; }
        public string? NameContains { get; set; }
    }
}
