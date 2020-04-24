using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.Extensions;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Queries.GetAllAlertsQuery
{
    public class GetAllAlertsQueryHandler : IRequestHandler<GetAllAlertsQuery, PaginatedResult<AlertVO>>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public GetAllAlertsQueryHandler(IBTBDbContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<PaginatedResult<AlertVO>> Handle(GetAllAlertsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<AlertVO> allUserAlerts =
                from alert in _context.Alerts
                .Include(x => x.SymbolPair).ThenInclude(x => x.BuySymbol)
                .Include(x => x.SymbolPair).ThenInclude(x => x.SellSymbol)
                where alert.UserId == _userAccessor.GetCurrentUserId()
                select _mapper.Map<AlertVO>(alert);

            int allUserAlertsCount = await allUserAlerts.CountAsync(cancellationToken);

            var list = allUserAlerts.ToList();

            return new PaginatedResult<AlertVO>()
            {
                Result = allUserAlerts.Paginate(request.Pagination),
                AllRecordsCount = allUserAlertsCount,
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }
    }
}
