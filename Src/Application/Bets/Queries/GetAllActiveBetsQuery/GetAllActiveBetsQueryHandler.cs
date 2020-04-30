using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.ValueObjects;
using BTB.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Bets.Queries.GetAllActiveBetsQuery
{
    public class GetAllActiveBetsQueryHandler : IRequestHandler<GetAllActiveBetsQuery, PaginatedResult<BetVO>>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;

        public GetAllActiveBetsQueryHandler(IBTBDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<BetVO>> Handle(GetAllActiveBetsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<BetVO> bets =
                from bet in _context.Bets
                .Include(bet => bet.SymbolPair).ThenInclude(sp => sp.BuySymbol)
                .Include(bet => bet.SymbolPair).ThenInclude(sp => sp.SellSymbol)
                select _mapper.Map<BetVO>(bet);

            int betsCount = await bets.CountAsync(cancellationToken);
            var list = bets.ToList();

            return new PaginatedResult<BetVO>()
            {
                Result = bets.Paginate(request.Pagination),
                AllRecordsCount = betsCount,
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }
    }
}
