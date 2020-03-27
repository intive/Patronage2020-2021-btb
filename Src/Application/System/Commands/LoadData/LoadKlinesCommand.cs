using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Behaviours;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.LoadData
{
    public class LoadKlinesCommand : IRequest
    {
        public TimestampInterval KlineType { get; set; }
        public int Amount { get; set; }

        public class LoadKlinesCommandHandler : IRequestHandler<LoadKlinesCommand>
        {
            private readonly IBinanceClient _client;
            private readonly IBTBDbContext _context;

            private DateTime _updateFrom;
            private DateTime _klineCallBuffer;

            private TimestampInterval _klineType;
            private KlineInterval _klineInterval;

            public LoadKlinesCommandHandler(IBinanceClient client, IBTBDbContext context)
            {
                _client = client;
                _context = context;
            }

            public async Task<Unit> Handle(LoadKlinesCommand request, CancellationToken cancellationToken)
            {
                if (request.Amount < 1)
                    return Unit.Value;

                SetConfigFromRequest(request);
                LoadKlinesToDb("BTC");

                return Unit.Value;
            }

            private void SetConfigFromRequest(LoadKlinesCommand request)
            {
                _updateFrom = DateTime.UtcNow.AddSeconds(-((double)request.KlineType * request.Amount + 60));
                _klineType = request.KlineType;
                _klineInterval = TimestampKlineIntervalConv.GetKlineInterval(request.KlineType);
                _klineCallBuffer = DateTime.UtcNow;                
            }

            private void LoadKlinesToDb(string buySymbolFilter)
            {
                var result = _context.SymbolPairs.ToList();                

                List<Kline> klines = new List<Kline>();

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                foreach (SymbolPair pair in result)
                {
                    Symbol buySymbol = _context.Symbols.FirstOrDefault(a => a.Id == pair.BuySymbolId);
                    Symbol sellSymbol = _context.Symbols.FirstOrDefault(a => a.Id == pair.SellSymbolId);

                    if (!string.IsNullOrEmpty(buySymbolFilter))
                    {
                        if (!string.Equals(buySymbol.SymbolName, buySymbolFilter))
                        {
                            continue;
                        }
                    }
                    
                    bool symbolsExist = (buySymbol != default(Symbol) && sellSymbol != default(Symbol));

                    if (!symbolsExist)
                        continue;

                    string pairName = string.Concat(buySymbol.SymbolName, sellSymbol.SymbolName);
                    var response = _client.GetKlines(pairName, _klineInterval, _updateFrom, _klineCallBuffer);
                    var klineList = response.Data;

                    foreach (BinanceKline kline in klineList)
                    {
                        var existingKline = _context.Klines.FirstOrDefault(k => 
                            k.OpenTimestamp == DateTimestampConv.GetTimestamp(kline.OpenTime) &&
                            k.SymbolPairId == pair.Id &&
                            k.DurationTimestamp == _klineType);

                        if (existingKline != default(Kline))
                        {
                            existingKline.OpenPrice = kline.Open;
                            existingKline.ClosePrice = kline.Close;
                            existingKline.LowestPrice = kline.Low;
                            existingKline.HighestPrice = kline.High;
                            existingKline.Volume = kline.Volume;
                            _context.Klines.Update(existingKline);
                        }
                        else
                        {
                            var newKline = new Kline()
                            {
                                SymbolPairId = pair.Id,
                                OpenTimestamp = Timestamp(kline.OpenTime),
                                DurationTimestamp = _klineType,
                                OpenPrice = kline.Open,
                                ClosePrice = kline.Close,
                                LowestPrice = kline.Low,
                                HighestPrice = kline.High,
                                Volume = kline.Volume
                            };

                            klines.Add(newKline);
                        }
                    }
                }
                stopWatch.Stop();
                _context.Klines.AddRange(klines.ToArray());
                _context.SaveChanges();
            }

            private long Timestamp(DateTime time)
            {
                return ((DateTimeOffset)time).ToUnixTimeSeconds();
            }
        }
    }
}
