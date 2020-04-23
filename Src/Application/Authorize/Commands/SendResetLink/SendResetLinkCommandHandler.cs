using BTB.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.SendResetLink
{
    public class SendResetLinkCommandHandler : IRequestHandler<SendResetLinkCommand>
    {
        private readonly IPasswordManager _passwordManager;

        public SendResetLinkCommandHandler(IPasswordManager passwordManager)
        {
            _passwordManager = passwordManager;
        }

        public Task<Unit> Handle(SendResetLinkCommand request, CancellationToken cancellationToken)
        {
            return _passwordManager.SendResetLink(request);
        }
    }
}
