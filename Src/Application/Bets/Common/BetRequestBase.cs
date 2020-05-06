namespace BTB.Application.Bets.Common
{
    public class BetRequestBase
    {
        public string SymbolPair { get; set; }
        public decimal Points { get; set; }
        public decimal LowerPriceThreshold { get; set; }
        public decimal UpperPriceThreshold { get; set; }
        public string RateType { get; set; }
        public string TimeInterval { get; set; }
    }
}
