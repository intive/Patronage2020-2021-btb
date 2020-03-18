using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Binance.Net.Interfaces;
using BTB.Application.Common.Exceptions;
using BTB.Domain.Entities;
using MediatR;

namespace BTB.Application.Dashboard.Queries.GetTopListQuery
{
    public class GetTopListQuery : IRequest<IEnumerable<BinanceSimpleElement>>
    {
        public class GetTopListQueryHandler : IRequestHandler<GetTopListQuery, IEnumerable<BinanceSimpleElement>>
        {
            private readonly IBinanceClient _client;

            public GetTopListQueryHandler(IBinanceClient client)
            {
                _client = client;
            }

            public async Task<IEnumerable<BinanceSimpleElement>> Handle(GetTopListQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.Get24HPricesListAsync(cancellationToken);

                if (result.Success)
                {
                    return result.Data
                            .Where(d => d.Symbol.Contains("BTC"))
                            .OrderByDescending(d => d.LastPrice)
                            .Select(d => new BinanceSimpleElement
                            {
                                Symbol = d.Symbol,
                                LastPrice = d.LastPrice
                            })
                            .Take(10);
                }

                throw new NotFoundException();
            }
        }
    }
}
