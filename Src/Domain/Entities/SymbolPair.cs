using System.Collections.Generic;

namespace BTB.Domain.Entities
{
    public class SymbolPair
    {
        public virtual IEnumerable<Kline> Klines { get; set; } = new List<Kline>();

        public int Id { get; set; }

        public int BuySymbolId { get; set; }
        public virtual Symbol BuySymbol { get; set; }

        public int SellSymbolId { get; set; }
        public virtual Symbol SellSymbol { get; set; }
    }
}
