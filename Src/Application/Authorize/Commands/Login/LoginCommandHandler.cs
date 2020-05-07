using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace BTB.Application.Authorize.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new BadRequestException("Incorrect username or password.");
            }
            
            var roles = await _userManager.GetRolesAsync(user);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (signInResult.Succeeded)
            {
                return _jwtGenerator.GenerateToken(user.Id, user.Email, request.UserName, roles);
            }

            throw new BadRequestException("Incorrect username or password.");
        }
    }
}
