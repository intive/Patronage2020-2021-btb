using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.GamblePoints.Queries.GetGamblePoints
{
    public class GetGamblePointsQueryHandler : IRequestHandler<GetGamblePointsQuery, GamblePointsVO>
    {
        private readonly IGamblePointManager _gamblePointManager;
        private readonly IUserAccessor _userAccessor;

        public GetGamblePointsQueryHandler(IGamblePointManager gamblePointManager, IUserAccessor userAccessor)
        {
            _gamblePointManager = gamblePointManager;
            _userAccessor = userAccessor;
        }

        public Task<GamblePointsVO> Handle(GetGamblePointsQuery request, CancellationToken cancellationToken)
        {
            GamblePoint userGamglePoints = _gamblePointManager.GetGamblePoint(_userAccessor.GetCurrentUserId());
            return Task.Run(() => new GamblePointsVO() { FreePoints = userGamglePoints.FreePoints, SealedPoints =  userGamglePoints.SealedPoints});
        }
    }
}
