using BTB.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Bets.Commands.DeleteBetCommand
{
    public class DeleteBetCommandHandler : IRequestHandler<DeleteBetCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IBetsManager _betsManager;

        public DeleteBetCommandHandler(IUserAccessor userAccessor, IBetsManager betsManager)
        {
            _userAccessor = userAccessor;
            _betsManager = betsManager;
        }

        public Task<Unit> Handle(DeleteBetCommand request, CancellationToken cancellationToken)
        {
            string userId = _userAccessor.GetCurrentUserId();

            return _betsManager.DeleteBetAsync(request, userId, cancellationToken);
        }
    }
}
