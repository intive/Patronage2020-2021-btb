using System;

namespace BTB.Domain.Entities
{
    public class BinanceSymbolPriceInTime
    {
        public DateTime Time { get; set; }
        public decimal Price { get; set; }

        public BinanceSymbolPriceInTime()
        {
        }
    }
}
