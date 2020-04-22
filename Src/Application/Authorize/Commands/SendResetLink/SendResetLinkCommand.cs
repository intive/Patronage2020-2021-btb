using MediatR;

namespace BTB.Application.Authorize.Commands.SendResetLink
{
    public class SendResetLinkCommand : IRequest
    {
        public string Email { get; set; }
    }
}
