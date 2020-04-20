using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Alerts.Common
{
    public class AlertRequestBase
    {
        public string SymbolPair { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public decimal Value { get; set; }
        public bool SendInBrowser { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
