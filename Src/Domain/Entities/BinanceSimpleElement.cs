namespace BTB.Domain.Entities
{
    public class BinanceSimpleElement
    {
        public string Symbol { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Volume { get; set; }
        
        public BinanceSimpleElement() { }
    };
}
