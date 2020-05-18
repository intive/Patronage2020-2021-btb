using BTB.Domain.Enums;

namespace BTB.Domain.ValueObjects
{
    public class AlertVO
    {
        public int Id { get; set; }
        public string SymbolPair { get; set; }
        public AlertCondition Condition { get; set; }
        public AlertValueType ValueType { get; set; }
        public decimal Value { get; set; }
        public decimal AdditionalValue { get; set; }
        public bool SendInBrowser { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool TriggerOnce { get; set; }
        public bool WasTriggered { get; set; }
        public bool IsDisabled { get => WasTriggered && TriggerOnce; }
    }
}
