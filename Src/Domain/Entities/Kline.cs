using BTB.Domain.Common;

namespace BTB.Domain.Entities
{
    public class Kline
    {
        public int Id { get; set; }
        public long OpenTimestamp { get; set; }
        public TimestampInterval DurationTimestamp { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal HighestPrice { get; set; }
        public decimal Volume { get; set; }

        public int SymbolPairId { get; set; }
        public virtual SymbolPair SymbolPair { get; set; }
    }
}
