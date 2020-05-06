using BTB.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.UpdateBetsCommand.CheckActiveBetsCommand
{
    public class CheckActiveBetsCommandHandler : IRequestHandler<CheckActiveBetsCommand>
    {
        private readonly IBetsManager _betsManager;

        public CheckActiveBetsCommandHandler(IBetsManager betsManager)
        {
            _betsManager = betsManager;
        }

        public async Task<Unit> Handle(CheckActiveBetsCommand request, CancellationToken cancellationToken)
        {
            await _betsManager.CheckActiveBetsAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
