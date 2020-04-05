﻿using AutoMapper;
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
        private readonly ICurrentUserIdentityService _userIdentity;

        public GetAllAlertsQueryHandler(IBTBDbContext context, IMapper mapper, ICurrentUserIdentityService userIdentity)
        {
            _context = context;
            _mapper = mapper;
            _userIdentity = userIdentity;
        }

        public async Task<PaginatedResult<AlertVO>> Handle(GetAllAlertsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<AlertVO> allUserAlerts =
                from alert in _context.Alerts
                .Include(x => x.SymbolPair).ThenInclude(x => x.BuySymbol)
                .Include(x => x.SymbolPair).ThenInclude(x => x.SellSymbol)
                where alert.UserId == _userIdentity.UserId
                select _mapper.Map<AlertVO>(alert);

            int allUserAlertsCount = await allUserAlerts.CountAsync(cancellationToken);

            var list = allUserAlerts.ToList();

            return new PaginatedResult<AlertVO>()
            {
                Result = allUserAlerts.Paginate(request.Pagination),
                AllRecorsCount = allUserAlertsCount,
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }
    }
}
