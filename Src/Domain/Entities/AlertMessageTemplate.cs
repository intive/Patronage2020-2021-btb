using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Entities
{
    public class AlertMessageTemplate
    {
        public int Id { get; set; }
        public AlertCondition Type { get; set; }
        public string Message { get; set; }

        public virtual IEnumerable<Alert> Alerts { get; set; } = new List<Alert>();
    }
}
