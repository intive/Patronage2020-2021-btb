using BTB.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Models
{
    public class AlertFormModel
    {
        public string SymbolPair { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public decimal Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
