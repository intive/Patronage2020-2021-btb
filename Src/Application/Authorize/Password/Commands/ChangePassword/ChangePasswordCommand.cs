using MediatR;

namespace BTB.Application.Authorize.Password.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}
