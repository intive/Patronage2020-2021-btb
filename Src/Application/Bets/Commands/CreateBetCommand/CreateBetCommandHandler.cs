using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Exceptions.BetsManager;
using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Bets.Commands.CreateBetCommand
{
    class CreateBetCommandHandler : IRequestHandler<CreateBetCommand, BetVO>
    {
        private readonly IBTBBinanceClient _client;
        private readonly IUserAccessor _userAccessor;
        private readonly IGamblePointManager _gamblePointsManager;
        private readonly IBetsManager _betsManager;

        public CreateBetCommandHandler(IBTBBinanceClient client, IUserAccessor userAccessor, IGamblePointManager gamblePointsManager, IBetsManager betsManager)
        {
            _client = client;
            _userAccessor = userAccessor;
            _gamblePointsManager = gamblePointsManager;
            _betsManager = betsManager;
        }

        public async Task<BetVO> Handle(CreateBetCommand request, CancellationToken cancellationToken)
        {
            if (_client.GetSymbolNames(request.SymbolPair) == null)
            {
                throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
            }

            string userId = _userAccessor.GetCurrentUserId();
            if (request.Points > _gamblePointsManager.GetNumberOfFreePoints(userId))
            {
                throw new BadRequestException($"User (id: {userId}) does not have enough points to place a bet with {request.Points} points.");
            }

            try
            {
                BetVO newBetVO = await _betsManager.CreateBetAsync(request, userId, cancellationToken);
                await _gamblePointsManager.SealGamblePoints(userId, newBetVO.Points, cancellationToken);
                return newBetVO;
            }
            catch (PriceRangeAboveLimitException ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
