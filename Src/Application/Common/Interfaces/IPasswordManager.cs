using BTB.Application.Authorize.Password.Commands.ChangePassword;
using MediatR;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IPasswordManager
    {
        Task<Unit> ChangePassword(ChangePasswordCommand changePasswordCommand);
    }
}
