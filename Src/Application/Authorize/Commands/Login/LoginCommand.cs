using BTB.Domain.Entities;
using MediatR;

namespace BTB.Application.Authorize.Commands.Login
{
    public class LoginCommand : IRequest<LoginParameters>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }        
    }
}