using BTB.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.Login
{
    public class LoginCommand : IRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand>
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly SignInManager<IdentityUser> _signInManager;

            public LoginCommandHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    throw new BadRequestException("Incorrect username or password.");
                }

                var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!singInResult.Succeeded)
                {
                    throw new BadRequestException("Incorrect username or password.");
                }

                await _signInManager.SignInAsync(user, request.RememberMe);

                return Unit.Value;
            }
        }
    }
}