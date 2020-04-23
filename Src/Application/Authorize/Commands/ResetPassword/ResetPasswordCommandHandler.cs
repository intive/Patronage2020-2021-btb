using BTB.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IPasswordManager _passwordManager;

        public ResetPasswordCommandHandler(IPasswordManager passwordManager)
        {
            _passwordManager = passwordManager;
        }

        public Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return _passwordManager.ResetPassword(request);
        }
    }
}
