using BTB.Application.Authorize.Commands.ChangePassword;
using BTB.Application.Authorize.Commands.ResetPassword;
using BTB.Application.Authorize.Commands.SendResetLink;
using MediatR;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IPasswordManager
    {
        Task<Unit> ChangePassword(ChangePasswordCommand changePasswordCommand);
        Task<Unit> SendResetLink(SendResetLinkCommand sendResetLinkCommand);
        Task<Unit> ResetPassword(ResetPasswordCommand resetPasswordCommand); 
    }
}
