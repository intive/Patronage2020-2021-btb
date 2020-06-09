using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot.MarketData;
using BTB.Application.Common.Behaviours;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.LoadData
{
    public class LoadKlinesCommand : IRequest
    {
        public TimestampInterval KlineType { get; set; }
        public int Amount { get; set; }
        public bool InitialCall { get; set; }

        public class LoadKlinesCommandHandler : IRequestHandler<LoadKlinesCommand>
        {
            private readonly IBinanceClient _client;
            private readonly IBTBDbContext _context;
            private readonly ILogger _logger;

            private DateTime _updateFrom;
            private DateTime _klineCallBuffer;

            private TimestampInterval _klineType;
            private KlineInterval _klineInterval;

            public LoadKlinesCommandHandler(IBinanceClient client, IBTBDbContext context, ILoggerFactory loggerFactory)
            {
                _client = client;
                _context = context;
                _logger = loggerFactory.CreateLogger<LoadKlinesCommandHandler>();
            }

            public async Task<Unit> Handle(LoadKlinesCommand request, CancellationToken cancellationToken)
            {
                if (request.InitialCall)
                {
                    if (_context.Klines.Where(k => k.DurationTimestamp == request.KlineType).ToList().Any())
                    {
                        return Unit.Value;
                    }
                }

                if (request.Amount < 1)
                    return Unit.Value;

                SetConfigFromRequest(request);
                await LoadKlinesToDb();

                return Unit.Value;
            }

            private void SetConfigFromRequest(LoadKlinesCommand request)
            {
                _updateFrom = DateTime.UtcNow.AddSeconds(-((double)request.KlineType * request.Amount + 60));
                _klineType = request.KlineType;
                _klineInterval = TimestampKlineIntervalConv.GetKlineInterval(request.KlineType);
                _klineCallBuffer = DateTime.UtcNow;                
            }

            private void ValidateResponse(HttpStatusCode statusCode, object error)
            {
                if (statusCode != HttpStatusCode.OK)
                {
                    throw new ServiceUnavailableException(error);
                }
            }

            private async Task LoadKlinesToDb()
            {
                var result = _context.SymbolPairs.ToList();

                List<Kline> klines = new List<Kline>();
                int updateCount = 0;

                foreach (SymbolPair pair in result)
                {
                    Symbol buySymbol = _context.Symbols.FirstOrDefault(a => a.Id == pair.BuySymbolId);
                    Symbol sellSymbol = _context.Symbols.FirstOrDefault(a => a.Id == pair.SellSymbolId);
                    
                    bool symbolsExist = (buySymbol != default(Symbol) && sellSymbol != default(Symbol));

                    if (!symbolsExist)
                        continue;

                    string pairName = string.Concat(buySymbol.SymbolName, sellSymbol.SymbolName);
                    var response = await _client.GetKlinesAsync(pairName, _klineInterval, _updateFrom, _klineCallBuffer);
                    ValidateResponse((HttpStatusCode)response.ResponseStatusCode, response.Error);

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
                            updateCount++;
                        }
                        else
                        {
                            var newKline = new Kline()
                            {
                                SymbolPairId = pair.Id,
                                OpenTimestamp = DateTimestampConv.GetTimestamp(kline.OpenTime),
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
                
                _context.Klines.AddRange(klines.ToArray());
                _context.SaveChanges();
                _logger.LogInformation($"{klines.Count} Klines: {_klineInterval.ToString()} has been added and {updateCount} updated.");
            }
        }
    }
}
