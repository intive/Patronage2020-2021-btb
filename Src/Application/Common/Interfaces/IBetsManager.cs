using BTB.Application.Bets.Commands.CreateBetCommand;
using BTB.Application.Bets.Commands.DeleteBetCommand;
using BTB.Application.Bets.Commands.UpdateBetCommand;
using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBetsManager
    {
        Task<BetVO> CreateBetAsync(CreateBetCommand request, string userId, CancellationToken cancellationToken);
        Task<BetVO> UpdateBetAsync(UpdateBetCommand request, string userId, CancellationToken cancellationToken);
        Task<Unit> DeleteBetAsync(DeleteBetCommand request, string userId, CancellationToken cancellationToken);
        Task<PaginatedResult<BetVO>> GetAllActiveBetsAsync(PaginationDto pagination, CancellationToken cancellationToken);
        Task CheckActiveBetsAsync(CancellationToken cancellationToken);
    }
}
