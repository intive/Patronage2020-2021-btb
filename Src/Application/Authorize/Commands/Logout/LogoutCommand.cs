using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
        {
            private readonly SignInManager<IdentityUser> _signInManager;

            public LogoutCommandHandler(SignInManager<IdentityUser> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
                await _signInManager.SignOutAsync();

                return Unit.Value;
            }
        }
    }
}
