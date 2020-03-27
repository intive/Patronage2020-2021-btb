using System;

namespace BTB.Domain.Entities
{
    public class AuditTrail
    {
        public long Id { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string UserId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
    }
}
