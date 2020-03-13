using System;

namespace BTB.Client.Pages.Dto
{
    public class BinanceSymbolPriceInTimeDto
    {
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal HighestPrice { get; set; }
    }
}
