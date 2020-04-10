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
        public KlineInterval KlineType { get; set; }
        public PaginationQuantity PaginationQuantity { get; set; }


        public class GetPriceHistoryQueryHandler : IRequestHandler<GetPriceHistoryQuery, IEnumerable<KlineVO>>
        {
            private readonly IBTBBinanceClient _client;

            public GetPriceHistoryQueryHandler(IBTBBinanceClient client)
            {
                _client = client;
            }

            public async Task<IEnumerable<KlineVO>> Handle(GetPriceHistoryQuery request, CancellationToken cancellationToken)
            {
                var klines = await _client.GetKlines(TimestampKlineIntervalConv.GetTimestampInterval(request.KlineType), (int)request.PaginationQuantity + request.AdditionalRows, request.PairName);
               
                if (klines.Any())
                {
                    return klines.OrderByDescending(k => k.OpenTime);
                }

                throw new BadRequestException("Error: no klines found");
            }
        }


    }
}
