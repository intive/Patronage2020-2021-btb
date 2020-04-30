using BTB.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IGamblePointManager
    {
        Task<Unit> InitGamblePoints(string userName, decimal amount, CancellationToken cancellationToken);
        Task<Unit> AddValueToAllGamblePoints(decimal value, CancellationToken cancellationToken);
        Task<Unit> ChangeFreePointsAmount(string userId, decimal amount, CancellationToken cancellationToken);
        Task<Unit> ChangeSealedPointsAmount(string userId, decimal amount, CancellationToken cancellationToken);
        Task<Unit> SealGamblePoints(string userId, decimal amount, CancellationToken cancellationToken);
        Task<Unit> UnsealGamblePoints(string userId, decimal amount, CancellationToken cancellationToken);
        decimal GetNumberOfFreePoints(string userId);
        GamblePoint GetGamblePoint(string userId);
    }
}
