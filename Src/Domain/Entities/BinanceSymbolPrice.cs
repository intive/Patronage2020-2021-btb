namespace BTB.Domain.Entities
{
    public class BinanceSymbolPrice
    {
        public string Symbol { get; set; }
        public decimal LastPrice { get; set; }
        
        public BinanceSymbolPrice() { }
    };
}
