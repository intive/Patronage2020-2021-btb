using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BTB.Domain.Extensions;
using System.Threading.Tasks;
using System.Threading;

namespace BTB.Application.FavoriteSymbolPairs.Queries.GetAllFavoriteSymbolPairs
{
    public class GetAllFavoriteSymbolPairsHandler : IRequestHandler<GetAllFavoriteSymbolPairsQuery, PaginatedResult<SimplePriceVO>>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserIdentityService _userIdentity;

        public GetAllFavoriteSymbolPairsHandler(IBTBDbContext context, IMapper mapper, ICurrentUserIdentityService userIdentity)
        {
            _context = context;
            _mapper = mapper;
            _userIdentity = userIdentity;
        }

        public async Task<PaginatedResult<SimplePriceVO>> Handle(GetAllFavoriteSymbolPairsQuery request, CancellationToken cancellationToken)
        {
            var userFavoriteSymbolPairs = _context.FavoriteSymbolPairs
                .Include(s => s.SymbolPair.BuySymbol)
                .Include(s => s.SymbolPair.SellSymbol)
                .Include(s => s.SymbolPair.Klines)
                .Where(pair => pair.ApplicationUserId == _userIdentity.UserId)
                .Select(pair => _mapper.Map<SimplePriceVO>(pair.SymbolPair));

            var userFavoriteSymbolPairsCount = await userFavoriteSymbolPairs.CountAsync(cancellationToken);

            return new PaginatedResult<SimplePriceVO>()
            {
                Result = userFavoriteSymbolPairs.ToList().Paginate(request.Pagination),
                AllRecorsCount = userFavoriteSymbolPairsCount,
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }
    }
}
