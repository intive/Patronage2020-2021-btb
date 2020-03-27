using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Alerts.Common
{
    public class AlertVm
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
