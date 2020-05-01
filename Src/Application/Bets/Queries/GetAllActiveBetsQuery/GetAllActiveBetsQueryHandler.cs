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
        private readonly IBetsManager _betsManager;

        public GetAllActiveBetsQueryHandler(IBetsManager betsManager)
        {
            _betsManager = betsManager;
        }

        public async Task<PaginatedResult<BetVO>> Handle(GetAllActiveBetsQuery request, CancellationToken cancellationToken)
        {
            return await _betsManager.GetAllActiveBetsAsync(request.Pagination, cancellationToken);
        }
    }
}
