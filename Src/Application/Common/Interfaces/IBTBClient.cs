using Binance.Net.Objects;
using BTB.Application.Common.Models;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBTBClient
    {
        string GetSymbolName(int symbolId);
        string GetSymbolName(int pairId, TradeSide tradeSide =TradeSide.Buy);
        IEnumerable<SymbolPair> GetSymbolPairs(string symbolName, TradeSide tradeSide);
        Task<IEnumerable<KlineVO>> GetKlines(TimestampInterval klineType, int amount, string filter = "");
        Task<List<KlineVO>> GetKlinesFrom(TimestampInterval klineType, TimestampInterval fromNow);
        Task<IEnumerable<KlineVO>> Get24HPricesListAsync();
        Task<IEnumerable<KlineVO>> FilterKlines(string filter, List<KlineVO> klines);
        Task<IEnumerable<SimplePriceVO>> ToSimplePrices(List<KlineVO> klines);
    }
}
