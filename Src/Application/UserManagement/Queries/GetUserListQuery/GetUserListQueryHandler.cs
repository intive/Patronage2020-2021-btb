using BTB.Application.Common.Models;
using BTB.Domain.ValueObjects;
using BTB.Domain.Extensions;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using MediatR;

namespace BTB.Application.UserManagement.Queries.GetUserListQuery
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedResult<ApplicationUserVO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserListQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedResult<ApplicationUserVO>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<ApplicationUserVO> usersVO = await IncludeRoles(users);

            return new PaginatedResult<ApplicationUserVO>
            {
                Result = usersVO.OrderByDescending(u => u.Id).Paginate(request.Pagination),
                AllRecordsCount = usersVO.Count(),
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }

        private async Task<List<ApplicationUserVO>> IncludeRoles(List<ApplicationUser> users)
        {
            var usersWithRoles = new List<ApplicationUserVO>();

            foreach (var user in users)
            {
                usersWithRoles.Add(new ApplicationUserVO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return usersWithRoles;
        }
    }
}
