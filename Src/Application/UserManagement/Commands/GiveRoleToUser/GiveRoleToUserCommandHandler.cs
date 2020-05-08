using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Exceptions;
using BTB.Domain.ValueObjects;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace BTB.Application.UserManagement.Commands.GiveRoleToUser
{
    public class GiveRoleToUserCommandHandler : IRequestHandler<GiveRoleToUserCommand, UserRolesVO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserAccessor _userAccessor;

        public GiveRoleToUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userAccessor = userAccessor;
        }

        public async Task<UserRolesVO> Handle(GiveRoleToUserCommand request, CancellationToken cancellationToken)
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

            string currUserName = _userAccessor.GetCurrentUserName();
            bool isUserAnAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isUserAnAdmin == true && request.UserName != currUserName)
            {
                throw new BadRequestException($"You are unable to grant roles to other admins.");
            }

            bool isUserInRole = await _userManager.IsInRoleAsync(user, request.Role);

            if (isUserInRole == true)
            {
                throw new BadRequestException($"User is already assigned to role \"{request.Role}\".");
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            IList<string> usersRoles = await _userManager.GetRolesAsync(user);
            var logoutRequired = request.UserName == currUserName? true : false;

            return new UserRolesVO { Roles = usersRoles, LogoutRequired = logoutRequired };
        }
    }
}
