using BTB.Application.Authorize.Common;
using BTB.Application.Authorize.Queries.GetAuthenticationInfo;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Queries.GetUserInfo
{
    public class GetAuthenticationInfoQueryHandler : IRequestHandler<GetAuthenticationInfoQuery, AuthenticationInfoDto>
    {
        public Task<AuthenticationInfoDto> Handle(GetAuthenticationInfoQuery request, CancellationToken cancellationToken)
        {
            var userInfo = new AuthenticationInfoDto
            {
                IsAuthenticated = request.User.Identity.IsAuthenticated,
                UserName = request.User.Identity.Name,
                ExposedClaims = request.User.Claims
                    .ToDictionary(c => c.Type, c => c.Value)
            };

            return Task.FromResult(userInfo);
        }
    }
}
