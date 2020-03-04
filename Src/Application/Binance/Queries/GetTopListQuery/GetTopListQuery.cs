using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTB.Domain.Entities;
using MediatR;
using Binance.Net;

namespace BTB.Application.Binance.Queries.GetTopListQuery
{
    public class GetTopListQuery : IRequest<IEnumerable<BinanceSymbolPrice>>
    {

        private readonly BinanceClient _client;

        public GetTopListQuery(BinanceClient client)
        {
            _client = client;
        }

        public class GetTopListQueryHandler : IRequestHandler<GetTopListQuery, IEnumerable<BinanceSymbolPrice>>
        {

            public async Task<IEnumerable<BinanceSymbolPrice>> Handle(GetTopListQuery request, CancellationToken cancellationToken)
            {
                var result = await request._client.Get24HPricesListAsync(cancellationToken);

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
