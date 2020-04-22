using BTB.Application.Common.Behaviours;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.ValueObjects;
using BTB.Domain.Extensions;
using BTB.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Binance.Net.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using AutoMapper;
using MediatR;

namespace BTB.Application.Details.Queries.GetPriceHistory
{
    public class GetPriceHistoryQueryHandler : IRequestHandler<GetPriceHistoryQuery, PaginatedResult<KlineVO>>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBinanceClient _apiClient;

        private const int AmountExtra = 50;

        public GetPriceHistoryQueryHandler(IBTBDbContext context, IMapper mapper, IBinanceClient apiClient)
        {
            _context = context;
            _mapper = mapper;
            _apiClient = apiClient;
        }

        public async Task<PaginatedResult<KlineVO>> Handle(GetPriceHistoryQuery request, CancellationToken cancellationToken)
        {
            if (request.DataSource == DetailsDataSource.Database)
            {
                var klines = _context.Klines
                    .Include(k => k.SymbolPair).ThenInclude(sp => sp.BuySymbol)
                    .Include(k => k.SymbolPair).ThenInclude(sp => sp.SellSymbol)
                    .AsEnumerable()
                    .Where(k => k.SymbolPair.PairName == request.PairName)
                    .Where(k => TimestampKlineIntervalConv.GetKlineInterval(k.DurationTimestamp) == request.KlineType)
                    .Select(k => _mapper.Map<KlineVO>(k))
                    .ToList();

                if (klines.Any())
                {
                    return new PaginatedResult<KlineVO>
                    {
                        Result = klines.OrderByDescending(k => k.OpenTime).Paginate(request.Pagination, AmountExtra),
                        AllRecordsCount = klines.Count(),
                        RecordsPerPage = (int)request.Pagination.Quantity
                    };
                }

                throw new BadRequestException("No klines found.");
            }
            /* 1. DetailsDataSource has only two states: Database and API. 
               2. Everything inside this else is temporary and eventually
                  will be removed. */
            else 
            {
                var result = await _apiClient.GetKlinesAsync(request.PairName, request.KlineType, ct: cancellationToken);
                if (result.Success)
                {
                    if (result.Data.Any())
                    {
                        var apiKlines = _mapper.Map<IEnumerable<KlineVO>>(result.Data).Reverse();
                        return new PaginatedResult<KlineVO>
                        {
                            Result = apiKlines.Paginate(request.Pagination, AmountExtra),
                            AllRecordsCount = apiKlines.Count(),
                            RecordsPerPage = (int)request.Pagination.Quantity
                        };
                    }
                }

                throw new BadRequestException("Could not get klines from Binance API.");
            }
        }


    }
}
