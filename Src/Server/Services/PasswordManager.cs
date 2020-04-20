using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.Authorize.Password.Commands.ChangePassword;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class PasswordManager : IPasswordManager
    {
        private readonly ICurrentUserIdentityService _currentUserIdentityService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PasswordManager(ICurrentUserIdentityService currentUserIdentityService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _currentUserIdentityService = currentUserIdentityService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Unit> ChangePassword(ChangePasswordCommand changePasswordCommand)
        {
            var user = await _userManager.GetUserAsync(_currentUserIdentityService.User);
            if(user == null)
            {
                throw new BadRequestException("Unable to find user.");
            }

            var checkCurrentPassword = await _signInManager.CheckPasswordSignInAsync(user, changePasswordCommand.CurrentPassword, false);
            if(!checkCurrentPassword.Succeeded)
            {
                throw new BadRequestException("Current password does not match.");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordCommand.CurrentPassword, changePasswordCommand.NewPassword);
            if(!result.Succeeded)
            {
                throw new BadRequestException("Password change failed.");
            }

            await _signInManager.RefreshSignInAsync(user);
            return Unit.Value;
        }
    }
}
