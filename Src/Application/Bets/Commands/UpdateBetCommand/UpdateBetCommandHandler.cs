using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Exceptions.BetsManager;
using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Bets.Commands.UpdateBetCommand
{
    public class UpdateBetCommandHandler : IRequestHandler<UpdateBetCommand, BetVO>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IBetsManager _betsManager;

        public UpdateBetCommandHandler(IUserAccessor userAccessor, IBetsManager betsManager)
        {
            _userAccessor = userAccessor;
            _betsManager = betsManager;
        }

        public Task<BetVO> Handle(UpdateBetCommand request, CancellationToken cancellationToken)
        {
            string userId = _userAccessor.GetCurrentUserId();

            try
            {
                return _betsManager.UpdateBetAsync(request, userId, cancellationToken);
            }
            catch(PriceRangeAboveLimitException ex)
            {
                throw new BadRequestException(ex.Message);
            }
                
        }
    }
}
