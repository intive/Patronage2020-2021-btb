using BTB.Application.Common.Exceptions;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace BTB.Application.UserManagement.Commands.GiveRoleToUser
{
    public class GiveRoleToUserCommandHandler : IRequestHandler<GiveRoleToUserCommand, IList<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GiveRoleToUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IList<string>> Handle(GiveRoleToUserCommand request, CancellationToken cancellationToken)
        {
            bool doesRoleExist = await _roleManager.RoleExistsAsync(request.Role);
            if (doesRoleExist == false)
            {
                throw new BadRequestException($"There is no role named \"{request.Role}\".");
            }

            ApplicationUser user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new BadRequestException($"There is no user named \"{request.UserName}\".");
            }

            bool isUserInRole = await _userManager.IsInRoleAsync(user, request.Role);
            if (isUserInRole == false)
            {
                await _userManager.AddToRoleAsync(user, request.Role);
            }

            IList<string> usersRoles = await _userManager.GetRolesAsync(user);
            return usersRoles;
        }
    }
}
