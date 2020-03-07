using BTB.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Domain.Entities;
using MediatR;
using System.Linq;

namespace BTB.Application.Details.Queries.GetPriceHistory
{
    public class GetPriceHistoryQuery : IRequest<IEnumerable<BinanceSymbolPriceInTime>>
    {
        public string Symbol { get; set; }
        public KlineInterval Interval { get; set; }

        public class GetPriceHistoryQueryHandler : IRequestHandler<GetPriceHistoryQuery, IEnumerable<BinanceSymbolPriceInTime>>
        {
            private readonly IBinanceClient _client;

            public GetPriceHistoryQueryHandler(IBinanceClient client)
            {
                _client = client;
            }

            public async Task<IEnumerable<BinanceSymbolPriceInTime>> Handle(GetPriceHistoryQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.GetKlinesAsync(request.Symbol, request.Interval);
               
                if (result.Success)
                {
                    return result.Data
                        .Reverse()
                        .Select(b => new BinanceSymbolPriceInTime
                        {
                            Time = b.CloseTime,
                            Price = b.Close
                        })
                        .Take(10);  
                }
                
                throw new System.Exception();
            }
        }


    }
}
