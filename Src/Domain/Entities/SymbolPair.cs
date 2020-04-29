using System.Collections.Generic;

namespace BTB.Domain.Entities
{
    public class SymbolPair
    {
        public virtual IEnumerable<Kline> Klines { get; set; } = new List<Kline>();
        public virtual IEnumerable<FavoriteSymbolPair> FavoritePairs { get; set; } = new List<FavoriteSymbolPair>();
        public virtual IEnumerable<Alert> Alerts { get; set; } = new List<Alert>();
        public virtual IEnumerable<Bet> Bets { get; set; } = new List<Bet>();

        public int Id { get; set; }

        public int BuySymbolId { get; set; }
        public virtual Symbol BuySymbol { get; set; }

        public int SellSymbolId { get; set; }
        public virtual Symbol SellSymbol { get; set; }

        public string PairName
        {
            get
            {
                return string.Concat(BuySymbol.SymbolName, SellSymbol.SymbolName);
            }
        }
    }
}
