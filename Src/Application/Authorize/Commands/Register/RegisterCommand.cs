using BTB.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.Register
{
    public class RegisterCommand : IRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
        {
            private readonly UserManager<IdentityUser> _userManager;

            public RegisterCommandHandler(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var user = new IdentityUser
                {
                    UserName = request.UserName
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);
                }

                return Unit.Value;
            }
        }
    }
}