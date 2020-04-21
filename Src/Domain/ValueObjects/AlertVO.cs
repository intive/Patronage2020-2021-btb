namespace BTB.Domain.ValueObjects
{
    public class AlertVO
    {
        public int Id { get; set; }
        public string SymbolPair { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public decimal Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool TriggerOnce { get; set; }
        public bool WasTriggered { get; set; }
        public bool IsDisabled { get => WasTriggered && TriggerOnce; }
    }
}
