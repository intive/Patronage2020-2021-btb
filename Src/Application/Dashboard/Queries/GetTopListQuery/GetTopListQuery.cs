using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Binance.Net.Interfaces;
using BTB.Application.Common.Exceptions;
using BTB.Domain.Entities;
using MediatR;
using BTB.Domain.ValueObjects;

namespace BTB.Application.Dashboard.Queries.GetTopListQuery
{
    public class GetTopListQuery : IRequest<IEnumerable<SimplePriceVO>>
    {
        public class GetTopListQueryHandler : IRequestHandler<GetTopListQuery, IEnumerable<SimplePriceVO>>
        {
            private readonly IBinanceClient _client;

            public GetTopListQueryHandler(IBinanceClient client)
            {
                _client = client;
            }

            public async Task<IEnumerable<SimplePriceVO>> Handle(GetTopListQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.Get24HPricesListAsync(cancellationToken);

                if (result.Success)
                {
                    return result.Data
                            .Where(d => d.Symbol.Contains("BTC"))
                            .OrderByDescending(d => d.LastPrice)
                            .Select(d => new SimplePriceVO
                            {
                                BuySymbolName = d.Symbol,
                                ClosePrice = d.LastPrice
                            })
                            .Take(10);
                }

                throw new NotFoundException();
            }
        }
    }
}
