using BTB.Application.Authorize.Common;
using MediatR;
using System.Security.Claims;

namespace BTB.Application.Authorize.Queries.GetAuthorizationInfo
{
    public class GetAuthorizationInfoQuery : IRequest<AuthorizationInfoDto>
    {
        public ClaimsPrincipal User { get; set; }
    }
}