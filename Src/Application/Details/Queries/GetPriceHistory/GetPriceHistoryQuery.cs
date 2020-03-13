using BTB.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Domain.Entities;
using MediatR;
using System.Linq;
using System;

namespace BTB.Application.Details.Queries.GetPriceHistory
{
    public class GetPriceHistoryQuery : IRequest<IEnumerable<BinanceSymbolPriceInTimeVm>>
    {
        public string Symbol { get; set; }
        public KlineInterval Interval { get; set; }

        public class GetPriceHistoryQueryHandler : IRequestHandler<GetPriceHistoryQuery, IEnumerable<BinanceSymbolPriceInTimeVm>>
        {
            private readonly IBinanceClient _client;

            public GetPriceHistoryQueryHandler(IBinanceClient client)
            {
                _client = client;
            }

            public async Task<IEnumerable<BinanceSymbolPriceInTimeVm>> Handle(GetPriceHistoryQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.GetKlinesAsync(request.Symbol, request.Interval, ct: cancellationToken);
               
                if (result.Success)
                {
                    return result.Data
                        .Reverse()
                        .Select(b => new BinanceSymbolPriceInTimeVm
                        {
                            OpenTime = b.OpenTime,
                            CloseTime = b.CloseTime,
                            OpenPrice = b.Open,
                            ClosePrice = b.Close,
                            LowestPrice = b.Low,
                            HighestPrice = b.High
                        })
                        .Take(10);  
                }

                throw new BadRequestException(result.Error.Message);
            }
        }


    }
}
