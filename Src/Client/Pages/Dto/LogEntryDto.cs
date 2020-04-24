using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Pages.Dto
{
    public class LogEntryDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public string TimeStampUtc { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
        public string Text { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
    }
}
