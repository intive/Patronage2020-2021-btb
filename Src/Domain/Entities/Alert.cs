using BTB.Domain.Common;
using BTB.Domain.Enums;

namespace BTB.Domain.Entities
{
    public class Alert : AuditableEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int SymbolPairId { get; set; }
        public SymbolPair SymbolPair { get; set; }
        public AlertCondition Condition { get; set; }
        public AlertValueType ValueType { get; set; }
        public decimal Value { get; set; }
        public decimal AdditionalValue { get; set; }
        public bool SendEmail { get; set; }
        public bool SendInBrowser { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool TriggerOnce { get; set; }
        public bool WasTriggered { get; set; }
        public bool IsDisabled { get => WasTriggered && TriggerOnce; }
    }
}
