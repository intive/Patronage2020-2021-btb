using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using BTB.Application.Common.Exceptions;
using BTB.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace BTB.Application.UserManagement.Commands.TakeRoleFromUser
{
    public class TakeRoleFromUserCommandHandler : IRequestHandler<TakeRoleFromUserCommand, IList<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private const int MinimalRoleCount = 1;

        public TakeRoleFromUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IList<string>> Handle(TakeRoleFromUserCommand request, CancellationToken cancellationToken)
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
                throw new BadRequestException($"User {user.UserName} doesn't have role \"{request.Role}\" so it can't be taken from him.");
            }
            else
            {
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
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, request.Role);
                }
            }

            IList<string> usersRoles = await _userManager.GetRolesAsync(user);
            return usersRoles;
        }
    }
}
