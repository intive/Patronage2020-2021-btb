using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using BTB.Application.Authorize.Commands.ChangePassword;
using BTB.Application.Authorize.Commands.SendResetLink;
using BTB.Application.Authorize.Commands.ResetPassword;
using Microsoft.AspNetCore.Http;

namespace BTB.Server.Services
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PasswordManager(IUserAccessor userAccessor, UserManager<ApplicationUser> userManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor, ILogger<PasswordManager> logger)
        {
            _userAccessor = userAccessor;
            _userManager = userManager;

            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            logger = logger;
        }

        public async Task<Unit> ChangePassword(ChangePasswordCommand changePasswordCommand)
        {
            var user = await _userManager.FindByIdAsync(_userAccessor.GetCurrentUserId());
            if (user == null)
            {
                throw new BadRequestException("Unable to find user.");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordCommand.CurrentPassword, changePasswordCommand.NewPassword);
            if (!result.Succeeded)
            {
                var errors = new StringBuilder();
                result.Errors.ToList().ForEach(e => errors.Append(e.Description));
                throw new BadRequestException(errors.ToString());
            }

            return Unit.Value;
        }

        public async Task<Unit> SendResetLink(SendResetLinkCommand sendResetLinkCommand)
        {
            var user = await _userManager.FindByEmailAsync(sendResetLinkCommand.Email);
            if(user == null)
            {
                return Unit.Value;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var webPath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

            var link = $"<a href='{webPath}/resetpassword/?email={sendResetLinkCommand.Email}&token={token}'>Reset password link.</a>";

            _emailService.Send(user.Email,"Reset password", link);

            return Unit.Value;
        }

        public async Task<Unit> ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordCommand.Email);
            if(user == null)
            {
                var e = new BadRequestException("Unable to reset password.");
                _logger.LogError(e, "Error: user {user} not found;");
                throw e;
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordCommand.Token, resetPasswordCommand.Password);
            if(!result.Succeeded)
            {
                var e = new BadRequestException("Unable to reset password.");
                _logger.LogError(e, "Error during reseting password");
                throw e;
            }

            return Unit.Value;
        }
    }
}
