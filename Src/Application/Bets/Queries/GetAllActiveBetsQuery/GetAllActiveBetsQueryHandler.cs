using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Bets.Queries.GetAllActiveBetsQuery
{
    public class GetAllActiveBetsQueryHandler : IRequestHandler<GetAllActiveBetsQuery, PaginatedResult<BetVO>>
    {
        private readonly IBetsManager _betsManager;
        private readonly IUserAccessor _userAccessor;

        public GetAllActiveBetsQueryHandler(IBetsManager betsManager, IUserAccessor userAccessor)
        {
            _betsManager = betsManager;
            _userAccessor = userAccessor;
        }

        public async Task<PaginatedResult<BetVO>> Handle(GetAllActiveBetsQuery request, CancellationToken cancellationToken)
        {
            string userId = null;
            if (request.OnlyUserBets)
            {
                userId = _userAccessor.GetCurrentUserId();
            }
            return await _betsManager.GetAllActiveBetsAsync(request.Pagination, userId, cancellationToken);
        }
    }
}
