using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class BinanceMiddleService : IBTBBinanceClient
    {
        private readonly IBTBDbContext _context;

        public BinanceMiddleService(IBTBDbContext context)
        {
            _context = context;
        }

        public string GetSymbolName(int symbolId)
        {
            try
            {
                return _context.Symbols.First(s => s.Id == symbolId).SymbolName;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public string GetSymbolName(int pairId, TradeSide side =TradeSide.Buy)
        {
            try
            {
                SymbolPair pair = _context.SymbolPairs.First(p => p.Id == pairId);
                string result = string.Empty;
                switch (side)
                {
                    case TradeSide.Buy:
                        {
                            result = _context.Symbols.FirstOrDefault(s => s.Id == pair.BuySymbolId).SymbolName;

                        }
                        break;
                    case TradeSide.Sell:
                        {
                            result = _context.Symbols.FirstOrDefault(s => s.Id == pair.SellSymbolId).SymbolName;
                        }
                        break;
                    case TradeSide.Both:
                        {
                            result = string.Concat(
                                    _context.Symbols.FirstOrDefault(s => s.Id == pair.BuySymbolId).SymbolName,
                                    _context.Symbols.FirstOrDefault(s => s.Id == pair.SellSymbolId).SymbolName
                                );
                        }
                        break;
                }
                return result;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public SymbolPair GetSymbolPair(int pairId)
        {
            var result = _context.SymbolPairs.FirstOrDefault(p => p.Id == pairId);
            return result;
        }

        public IEnumerable<SymbolPair> GetSymbolPairs(string symbolName, TradeSide tradeSide)
        {
            IEnumerable<SymbolPair> result = new List<SymbolPair>();
            Symbol symbol = _context.Symbols.FirstOrDefault(s => s.SymbolName == symbolName);

            if (symbol != default(Symbol))
            {
                switch (tradeSide)
                {
                    case TradeSide.Buy:
                        {
                            result = _context.SymbolPairs.Where(s => s.BuySymbolId == symbol.Id).ToList();
                        }
                        break;
                    case TradeSide.Sell:
                        {
                            result = _context.SymbolPairs.Where(s => s.SellSymbolId == symbol.Id).ToList();
                        }
                        break;
                    case TradeSide.Both:
                        {
                            result = _context.SymbolPairs.Where(s => s.BuySymbolId == symbol.Id).ToList();
                            result = result.Concat(_context.SymbolPairs.Where(s => s.BuySymbolId == symbol.Id).ToList());
                        }
                        break;
                }
            }

            return result;
        }

        private List<KlineVO> KlinesToValueObject(List<Kline> klines)
        {
            return klines.Select(k => new KlineVO()
            {
                BuySymbolName = GetSymbolName(k.SymbolPairId, TradeSide.Buy),
                SellSymbolName = GetSymbolName(k.SymbolPairId, TradeSide.Sell),
                ClosePrice = k.ClosePrice,
                OpenPrice = k.OpenPrice,
                LowestPrice = k.LowestPrice,
                HighestPrice = k.HighestPrice,
                OpenTime = DateTimestampConv.GetDateTime(k.OpenTimestamp),
                CloseTime = DateTimestampConv.GetDateTime(k.OpenTimestamp + (long)k.DurationTimestamp),
                Volume = k.Volume
            }).ToList();
        }

        public async Task<IEnumerable<KlineVO>> GetKlines(TimestampInterval klineType, int amount, string filter ="")
        {
            long now = DateTimestampConv.GetTimestamp(DateTime.UtcNow);

            long timeBack = (long)klineType * amount;
            var klines = _context.Klines.Where(k =>
                (k.DurationTimestamp == klineType) &&
                (now - k.OpenTimestamp <= (long)klineType * (long)amount)).ToList();

            var result = KlinesToValueObject(klines).AsEnumerable();

            if (!string.IsNullOrEmpty(filter))
            {
                 result = await FilterKlines(filter, result.ToList());
            }

            if (result.Count() > amount)
            {
                return result.TakeLast(amount);
            }

            return result;
        }

        public async Task<List<KlineVO>> GetKlinesFrom(TimestampInterval klineType, TimestampInterval fromNow)
        {
            long now = DateTimestampConv.GetTimestamp(DateTime.UtcNow);

            var klines = _context.Klines.Where(k =>
                (
                    (k.DurationTimestamp == klineType) &&
                    (now - k.OpenTimestamp <= (long)fromNow)
                )).ToList();

            var result = KlinesToValueObject(klines);

            return result;
        }

        public async Task<IEnumerable<KlineVO>> Get24HPricesListAsync()
        {
            return await GetKlinesFrom(TimestampInterval.OneDay, TimestampInterval.OneDay);
        }

        public async Task<IEnumerable<KlineVO>> FilterKlines(string filter, List<KlineVO> klines)
        {
            var result = new List<KlineVO>();
            filter = filter.ToUpper();

            foreach (KlineVO kline in klines)
            {
                if (
                    kline.BuySymbolName.Contains(filter) ||
                    kline.SellSymbolName.Contains(filter) ||
                    kline.PairName.Contains(filter)
                   )
                    result.Add(kline);
            }

            return result.AsEnumerable();
        }

        public async Task<IEnumerable<SimplePriceVO>> ToSimplePrices(List<KlineVO> klines)
        {
            return klines.Select(k => new SimplePriceVO()
            {
                BuySymbolName = k.BuySymbolName,
                SellSymbolName = k.SellSymbolName,
                ClosePrice = k.ClosePrice,
                Volume = k.Volume
            });
        }

        public SymbolPairVO GetSymbolNames(string pairName, string wantedBuySymbol = "")
        {
            var result = new SymbolPairVO();
            var symbols = _context.Symbols.ToList();

            foreach (Symbol symbol in symbols)
            {
                try
                {
                    string firstSymbol = pairName.Substring(0, symbol.SymbolName.Length);
                    if (string.IsNullOrEmpty(wantedBuySymbol) && symbol.SymbolName == firstSymbol)
                    {
                        result.BuySymbolName = symbol.SymbolName;
                        break;
                    }
                    else if (symbol.SymbolName == wantedBuySymbol)
                    {
                        result.BuySymbolName = symbol.SymbolName;
                        break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }

            if (result.BuySymbolName == null)
            {
                return null;
            }

            string secondSymbol = pairName.Substring(result.BuySymbolName.Length);

            foreach (Symbol symbol in symbols)
            {
                try
                {
                    if (symbol.SymbolName == secondSymbol)
                    {
                        result.SellSymbolName = symbol.SymbolName;
                        break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }

            if (result.SellSymbolName == null)
            {
                return null;
            }

            return result;
        }

        /* Please do not delete - maybe we can use it to calculate 15m-4h klines
         * 
        public async Task<IEnumerable<SimplePriceVO>> Calculate24hPricesAsync(string buySymbolName)
        {
            List<SimplePriceVO> result = new List<SimplePriceVO>();

            IEnumerable<KlineVO> klines = await GetKlinesFrom(buySymbolName, KlineInterval.FiveMinutes, TimestampInterval.OneDay);
            IEnumerable<SymbolPair> symbolPairs = GetSymbolPairs(buySymbolName, TradeSide.Buy);

            Dictionary<int, string> symbolNames = _context.Symbols.ToDictionary(s => s.Id, s => s.SymbolName);
            Dictionary<string, decimal> symbolVolumes = new Dictionary<string, decimal>();
            Dictionary<string, List<KlineVO>> klinesBySellSymbols = new Dictionary<string, List<KlineVO>>();

            foreach (KlineVO kline in klines)
            {
                string symbol = kline.SellSymbolName;
                decimal volume = 0;

                symbolVolumes.TryGetValue(symbol, out volume);
                volume += kline.Volume;
                symbolVolumes[symbol] = volume;

                if (!klinesBySellSymbols.ContainsKey(symbol))
                {
                    klinesBySellSymbols.Add(symbol, new List<KlineVO>());
                }

                if (symbol == kline.SellSymbolName)
                {
                    klinesBySellSymbols[symbol].Add(kline);
                }
            }

            foreach (SymbolPair pair in symbolPairs)
            {
                string sellSymbolName = symbolNames.GetValueOrDefault(pair.SellSymbolId);
                decimal volume = symbolVolumes.GetValueOrDefault(sellSymbolName);
                
                decimal lastPrice = 0;
                try
                {
                    var sortedKlines = klinesBySellSymbols.GetValueOrDefault(sellSymbolName).OrderByDescending(k => k.ClosePrice);
                    KlineVO first = sortedKlines.FirstOrDefault();
                    lastPrice = first == default(KlineVO) ? 0 : first.ClosePrice;
                } catch {
                    var xd = "";
                }

                result.Add(new SimplePriceVO()
                {
                    BuySymbolName = buySymbolName,
                    SellSymbolName = sellSymbolName,
                    Price = lastPrice,
                    Volume = volume
                });
            }

            return result;          
        }*/
    }
}
