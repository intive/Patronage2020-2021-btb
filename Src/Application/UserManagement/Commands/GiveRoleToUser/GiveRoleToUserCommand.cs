using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.UserManagement.Commands.GiveRoleToUser
{
    public class GiveRoleToUserCommand : IRequest<UserRolesVO>
    {
        public string Role { get; set; }
        public string UserName { get; set; }

        public GiveRoleToUserCommand()
        {
            Role = null;
            UserName = null;
        }
    }
}
