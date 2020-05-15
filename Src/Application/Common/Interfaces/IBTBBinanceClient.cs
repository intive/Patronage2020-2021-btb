using Binance.Net.Objects;
using BTB.Application.Common.Models;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBTBBinanceClient
    {
        string GetSymbolName(int symbolId);
        string GetSymbolName(int pairId, TradeSide tradeSide =TradeSide.Buy);
        IEnumerable<SymbolPair> GetSymbolPairs(string symbolName, TradeSide tradeSide);
        Task<IEnumerable<KlineVO>> GetKlines(TimestampInterval klineType, int amount, string filter = "");
        Task<List<Kline>> GetKlinesFrom(TimestampInterval klineType, TimestampInterval fromNow);
        Task<IEnumerable<Kline>> Get24HPricesListAsync();
        Task<List<Kline>> FilterKlines(string filter, List<Kline> klines);
        List<KlineVO> KlinesToValueObject(List<Kline> klines);
        Task<IEnumerable<SimplePriceVO>> ToSimplePrices(List<KlineVO> klines);
        SymbolPairVO GetSymbolNames(string pairName, string wantedBuySymbol = "");
        Task<SymbolPair> GetSymbolPairByName(string pairName);
        Task<Kline> GetLastKlineBySymboPair(SymbolPair symbolPair, TimestampInterval timestampInterval);
    }
}
