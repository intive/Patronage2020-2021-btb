using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System.Linq;

namespace BTB.Application.UserManagement.Queries.GetRoleList
{
    public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery, List<string>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetRoleListQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<string>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            List<string> roleNames = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            return roleNames;
        }
    }
}
