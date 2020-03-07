using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Binance.Net.Interfaces;
using BTB.Domain.Entities;
using MediatR;

namespace BTB.Application.Dashboard.Queries.GetTopListQuery
{
    public class GetTopListQuery : IRequest<IEnumerable<BinanceSymbolPrice>>
    {
        public class GetTopListQueryHandler : IRequestHandler<GetTopListQuery, IEnumerable<BinanceSymbolPrice>>
        {
            private readonly IBinanceClient _client;

            public GetTopListQueryHandler(IBinanceClient client)
            {
                _client = client;
            }

            public async Task<IEnumerable<BinanceSymbolPrice>> Handle(GetTopListQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.Get24HPricesListAsync(cancellationToken);

                if (result.Success)
                {
                    return result.Data
                            .Where(d => d.Symbol.Contains("BTC"))
                            .OrderByDescending(d => d.LastPrice)
                            .Select(d => new BinanceSymbolPrice
                            {
                                Symbol = d.Symbol,
                                LastPrice = d.LastPrice
                            })
                            .Take(10);
                }

                return new List<BinanceSymbolPrice>();
            }
        }
    }
}
