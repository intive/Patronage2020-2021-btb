using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BTB.Domain.Extensions;
using System.Threading.Tasks;
using System.Threading;
using BTB.Domain.ValueObjects;

namespace BTB.Application.FavoriteSymbolPairs.Queries.GetAllFavoriteSymbolPairs
{
    public class GetAllFavoriteSymbolPairsHandler : IRequestHandler<GetAllFavoriteSymbolPairsQuery, PaginatedResult<DashboardPairVO>>
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

        public async Task<PaginatedResult<DashboardPairVO>> Handle(GetAllFavoriteSymbolPairsQuery request, CancellationToken cancellationToken)
        {
            var userFavoriteSymbolPairs = _context.FavoriteSymbolPairs
                .Include(p => p.SymbolPair.BuySymbol)
                .Include(p => p.SymbolPair.SellSymbol)
                .Include(p => p.SymbolPair.Klines)
                .Where(p => p.ApplicationUserId == _userIdentity.UserId)
                .Select(p => _mapper.Map<DashboardPairVO>(p.SymbolPair))
                .ToList();

            userFavoriteSymbolPairs.ForEach(p => p.IsFavorite = !(_context.FavoriteSymbolPairs.Find(_userIdentity.UserId, p.Id) == null));

            if (request.Name != null)
            {
                userFavoriteSymbolPairs = userFavoriteSymbolPairs
                    .Where(pair => pair.PairName.Contains(request.Name.ToUpper())).ToList();
            }
          
            var userFavoriteSymbolPairsCount = await Task.Run(() => userFavoriteSymbolPairs.Count());

            return new PaginatedResult<DashboardPairVO> ()
            {
                Result = userFavoriteSymbolPairs.Paginate(request.Pagination),
                AllRecordsCount = userFavoriteSymbolPairsCount,
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }
    }
}
