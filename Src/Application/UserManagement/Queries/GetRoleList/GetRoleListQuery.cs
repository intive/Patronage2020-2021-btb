using System.Collections.Generic;
using MediatR;

namespace BTB.Application.UserManagement.Queries.GetRoleList
{
    public class GetRoleListQuery : IRequest<List<string>>
    {
    }
}
