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
    public class GetTopListQuery : IRequest<List<BinanceSymbolPrice>>
    {

        private readonly BinanceClient _client;

        public GetTopListQuery(BinanceClient client)
        {
            _client = client;
        }

        public class GetTopListQueryHandler : IRequestHandler<GetTopListQuery, List<BinanceSymbolPrice>>
        {

            public async Task<List<BinanceSymbolPrice>> Handle(GetTopListQuery request, CancellationToken cancellationToken)
            {
                var responses = new List<BinanceSymbolPrice>();

                var result = await request._client.Get24HPricesListAsync(cancellationToken);

                if (result.Success)
                {
                    var data = result.Data;
                    var selectedData =
                        data.Where(d => d.Symbol.Contains("BTC"))
                            .Select(d => new BinanceSymbolPrice
                            {
                                Symbol = d.Symbol,
                                LastPrice = d.LastPrice
                            })
                            .OrderByDescending(d => d.LastPrice)
                            .Take(10).ToList();

                    responses.AddRange(selectedData);
                }

                return responses;
            }
        }
    }
}
