using BTB.Application.Bets.Commands.CreateBetCommand;
using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBetsManager
    {
        Task<BetVO> CreateBetAsync(CreateBetCommand request, string userId, CancellationToken cancellationToken);
        Task<PaginatedResult<BetVO>> GetAllActiveBetsAsync(PaginationDto pagination, CancellationToken cancellationToken);
        Task CheckBetsAsync(CancellationToken cancellationToken);
    }
}
