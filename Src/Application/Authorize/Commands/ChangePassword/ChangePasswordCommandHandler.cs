using BTB.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IPasswordManager _passwordManager;

        public ChangePasswordCommandHandler(IPasswordManager passwordManager)
        {
            _passwordManager = passwordManager;
        }

        public Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return _passwordManager.ChangePassword(request);
        }
    }
}
