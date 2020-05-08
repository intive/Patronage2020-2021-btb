using System.Collections.Generic;

namespace BTB.Domain.ValueObjects
{
    public class UserRolesVO
    {
        public IList<string> Roles { get; set; }
        public bool LogoutRequired { get; set; }
    }
}
