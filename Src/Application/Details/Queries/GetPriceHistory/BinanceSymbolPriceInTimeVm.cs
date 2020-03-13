using System;

namespace BTB.Application.Details.Queries.GetPriceHistory
{
    public class BinanceSymbolPriceInTimeVm
    {
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal HighestPrice { get; set; }
    }
}
