using BTB.Application.Common.Exceptions;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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