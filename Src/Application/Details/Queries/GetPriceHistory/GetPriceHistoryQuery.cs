using BTB.Application.Common.Exceptions;
using BTB.Domain.Common.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using MediatR;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.ValueObjects;
using BTB.Domain.Common;
using BTB.Application.Common.Behaviours;

namespace BTB.Application.Details.Queries.GetPriceHistory
{
    public class GetPriceHistoryQuery : IRequest<IEnumerable<KlineVO>>
    {
        public string PairName { get; set; }
        public int AdditionalRows { get; set; }
        public PaginationQuantity PaginationQuantity { get; set; }
        public KlineInterval KlineType { get; set; }
        public DetailsDataSource DataSource { get; set; }

        public class GetPriceHistoryQueryHandler : IRequestHandler<GetPriceHistoryQuery, IEnumerable<KlineVO>>
        {
            private readonly IBTBBinanceClient _client;

            private readonly IBinanceClient _apiClient;

            public GetPriceHistoryQueryHandler(IBTBBinanceClient client, IBinanceClient apiClient)
            {
                _client = client;
                _apiClient = apiClient;
            }

            public async Task<IEnumerable<KlineVO>> Handle(GetPriceHistoryQuery request, CancellationToken cancellationToken)
            {
                if (request.DataSource == DetailsDataSource.Database)
                {
                    var klines = await _client.GetKlines(TimestampKlineIntervalConv.GetTimestampInterval(request.KlineType), (int)request.PaginationQuantity + request.AdditionalRows, request.PairName);

                    if (klines.Any())
                    {
                        return klines.OrderByDescending(k => k.OpenTime);
                    }

                    throw new BadRequestException("No klines found.");
                }
                else // DetailsDataSource has only two states: Database and API
                {
                    var result = await _apiClient.GetKlinesAsync(request.PairName, request.KlineType, ct: cancellationToken);
                    if (result.Success)
                    {
                        return result.Data
                            .Reverse()
                            .Select(b => new KlineVO
                            {
                                OpenTime = b.OpenTime,
                                CloseTime = b.CloseTime,
                                OpenPrice = b.Open,
                                ClosePrice = b.Close,
                                LowestPrice = b.Low,
                                HighestPrice = b.High
                            }).Take((int)request.PaginationQuantity + request.AdditionalRows);
                    }

                    throw new BadRequestException("Could not get klines from Binance API.");
                }
            }
        }


    }
}