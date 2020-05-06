namespace BTB.Domain.ValueObjects
{
    public class BetVO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SymbolPair { get; set; }
        public decimal Points { get; set; }
        public decimal LowerPriceThreshold { get; set; }
        public decimal UpperPriceThreshold { get; set; }
        public string StartedAt { get; set; }
        public string RateType { get; set; }
        public string TimeInterval { get; set; }
        public long KlineOpenTimestamp { get; set; }
        public bool IsActive { get; set; }
        public bool IsEditable { get; set; }
    }
}
