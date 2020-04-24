using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BTB.Domain.Extensions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using BTB.Application.Common.Models;

namespace BTB.Application.Dashboard.Queries.GetListQuery
{
    public class GetListQueryHandler : IRequestHandler<GetListQuery, PaginatedResult<DashboardPairVO>>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public GetListQueryHandler(IBTBDbContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<PaginatedResult<DashboardPairVO>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var symbolPairs = _context.SymbolPairs
                .Include(p => p.BuySymbol)
                .Include(p => p.SellSymbol)
                .Include(p => p.Klines)
                .Select(p => _mapper.Map<DashboardPairVO>(p))
                .ToList();

            symbolPairs.ForEach(p => p.IsFavorite = !(_context.FavoriteSymbolPairs.Find(_userAccessor.GetCurrentUserId(), p.Id) == null));

            if (request.Name != null)
            {
                symbolPairs = symbolPairs
                    .Where(pair => pair.PairName.Contains(request.Name.ToUpper())).ToList();
            }

            var symbolPairsCount = await Task.Run(() => symbolPairs.Count());

            return new PaginatedResult<DashboardPairVO>()
            {
                Result = symbolPairs.Paginate(request.Pagination),
                AllRecordsCount = symbolPairsCount,
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }
    }
}
