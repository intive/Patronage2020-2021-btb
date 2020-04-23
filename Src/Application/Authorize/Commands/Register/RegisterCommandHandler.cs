using BTB.Application.Common.Exceptions;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Linq;
using MediatR;

namespace BTB.Application.Authorize.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = new StringBuilder();
                result.Errors.ToList().ForEach(e => errors.Append(e.Description));
                throw new BadRequestException(errors.ToString());
            }

            return Unit.Value;
        }
    }
}
