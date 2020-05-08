using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using BTB.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace BTB.Application.UserManagement.Commands.TakeRoleFromUser
{
    public class TakeRoleFromUserCommandHandler : IRequestHandler<TakeRoleFromUserCommand, UserRolesVO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserAccessor _userAccessor;

        private const int MinimalRoleCount = 1;

        public TakeRoleFromUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userAccessor = userAccessor;
        }

        public async Task<UserRolesVO> Handle(TakeRoleFromUserCommand request, CancellationToken cancellationToken)
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
                throw new BadRequestException($"You are unable to remove roles from other admins.");
            }

            bool isUserInRole = await _userManager.IsInRoleAsync(user, request.Role);

            if (isUserInRole == false)
            {
                throw new BadRequestException($"User {user.UserName} doesn't have role \"{request.Role}\" so it can't be taken from him.");
            }

            /* Added to make sure that seeded admin will remain an admin no matter what
                 * and to guarantee there would always be at least one admin. */
            if ((request.Role.ToLower() == "admin") && (user.UserName == _configuration["MainAdmin:username"]))
            {
                throw new BadRequestException($"Not allowed to take away Admin role from main administrator.");
            }

            int rolesCount = (await _userManager.GetRolesAsync(user)).Count;

            if (rolesCount == MinimalRoleCount)
            {
                throw new BadRequestException($"\"{request.Role}\" is the only role user {user.UserName} has. User needs at least one role.");
            }

            await _userManager.RemoveFromRoleAsync(user, request.Role);

            var logoutRequired = request.UserName == currUserName ? true : false;
            IList<string> usersRoles = await _userManager.GetRolesAsync(user);

            return new UserRolesVO { Roles = usersRoles, LogoutRequired = logoutRequired };
        }
    }
}
