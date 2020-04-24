using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Entities
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public string Category { get; set; }
        public LogLevel Level { get; set; }
        public string Text { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
    }
}
