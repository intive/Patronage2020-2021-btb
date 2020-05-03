using BTB.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.UpdateBetsCommand
{
    public class CheckBetsCommandHandler : IRequestHandler<CheckBetsCommand>
    {
        private readonly IBetsManager _betsManager;

        public CheckBetsCommandHandler(IBetsManager betsManager)
        {
            _betsManager = betsManager;
        }

        public async Task<Unit> Handle(CheckBetsCommand request, CancellationToken cancellationToken)
        {
            await _betsManager.CheckBetsAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
