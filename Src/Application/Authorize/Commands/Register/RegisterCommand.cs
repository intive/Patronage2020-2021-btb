using MediatR;

namespace BTB.Application.Authorize.Commands.Register
{
    public class RegisterCommand : IRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}