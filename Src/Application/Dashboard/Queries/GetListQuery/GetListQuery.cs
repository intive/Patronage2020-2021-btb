using System.Collections;
using System.Collections.Generic;
using MediatR;
using System.Linq;
using System.Threading;
using BTB.Domain.Entities;
using Binance.Net.Interfaces;
using System.Threading.Tasks;
using BTB.Application.Common.Exceptions;

namespace BTB.Application.Dashboard.Queries.GetListQuery
{
    public class GetListQuery : IRequest<IEnumerable<BinanceSimpleElement>>
    {
        public string Name { get; set; }

        public class GetListQueryHandler : IRequestHandler<GetListQuery, IEnumerable<BinanceSimpleElement>>
        {
            private readonly IBinanceClient _client;

            public GetListQueryHandler(IBinanceClient client)
            {
                _client = client;
            }

            public async Task<IEnumerable<BinanceSimpleElement>> Handle(GetListQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.Get24HPricesListAsync(cancellationToken);

                if (result.Success)
                {
                    var data = result.Data
                        .Select(d => new BinanceSimpleElement
                        {
                            Symbol = d.Symbol,
                            LastPrice = d.LastPrice,
                            Volume = d.Volume
                        });

                    if (!string.IsNullOrEmpty(request.Name))
                    {
                        data = data
                            .Where(d => d.Symbol.Contains(request.Name.ToUpper()));
                    }

                    return data;
                }

                throw new NotFoundException();
            }
        }
    }
}
