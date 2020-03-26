using BTB.Application.Authorize.Common;
using MediatR;

namespace BTB.Application.Authorize.Commands.Login
{
    public class LoginCommand : IRequest<LoginParametersDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }        
    }
}