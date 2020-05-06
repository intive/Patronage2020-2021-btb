using BTB.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.AddValueToAllGamblePoints
{
    public class AddValueToAllGamblePointsCommandHandler : IRequestHandler<AddValueToAllGamblePointsCommand>
    {
        private readonly IGamblePointManager _pointManager;

        public AddValueToAllGamblePointsCommandHandler(IGamblePointManager pointManager)
        {
            _pointManager = pointManager;
        }

        public async Task<Unit> Handle(AddValueToAllGamblePointsCommand request, CancellationToken cancellationToken)
        {
            return await _pointManager.AddValueToAllGamblePoints(request.Amount, cancellationToken);
        }
    }
}
