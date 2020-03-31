using BTB.Application.Authorize.Common;
using MediatR;
using System.Security.Claims;

namespace BTB.Application.Authorize.Queries.GetUserInfo
{
    public class GetUserInfoQuery : IRequest<UserInfoDto>
    {
        public ClaimsPrincipal User { get; set; }
    }
}