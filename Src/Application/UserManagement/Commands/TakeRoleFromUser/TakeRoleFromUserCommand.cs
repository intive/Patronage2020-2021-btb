using System.Collections.Generic;
using MediatR;

namespace BTB.Application.UserManagement.Commands.TakeRoleFromUser
{
    public class TakeRoleFromUserCommand : IRequest<IList<string>>
    {
        public string Role { get; set; }
        public string UserName { get; set; }

        public TakeRoleFromUserCommand()
        {
            Role = null;
            UserName = null;
        }
    }
}
