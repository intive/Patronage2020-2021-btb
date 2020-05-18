using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Bets.Queries.GetAllActiveBetsQuery
{
    public class GetAllActiveBetsQuery : IRequest<PaginatedResult<BetVO>>
    {
        public PaginationDto Pagination { get; set; }
        public bool OnlyUserBets { get; set; }
    }
}
