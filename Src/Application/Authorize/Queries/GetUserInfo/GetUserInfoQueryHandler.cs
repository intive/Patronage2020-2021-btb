using BTB.Application.Authorize.Common;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Queries.GetUserInfo
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, AuthorizationInfoDto>
    {
        public Task<AuthorizationInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var userInfo = new AuthorizationInfoDto
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