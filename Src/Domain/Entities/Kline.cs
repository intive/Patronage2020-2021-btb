using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Entities
{
    public class Kline
    {
        public int Id { get; set; }

        public string OpenTimestamp { get; set; }
        public string CloseTimestamp { get; set; }
        public string OpenPrice { get; set; }
        public string ClosePrice { get; set; }
        public string LowestPrice { get; set; }
        public string HighestPrice { get; set; }

        public int BuySymbolId { get; set; }
        public Symbol BuySymbol { get; set; }
        public int SellSymbolId { get; set; }
        public Symbol SellSymbol { get; set; }
    }
}
