using System;

namespace BTB.Application.System.Common
{
    public class AuditTrailVm
    {
        public string Column { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
    }
}
