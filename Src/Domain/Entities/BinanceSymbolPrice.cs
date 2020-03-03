namespace BTB.Domain.Entities
{
    public class BinanceSymbolPrice
    {
        // public because Dashboard.razor requires access
        public string Symbol { get; set; }
        public decimal LastPrice { get; set; }
        
        // seem unused but it's required for GetJsonAsync<>()
        public BinanceSymbolPrice() { }
    };
}
