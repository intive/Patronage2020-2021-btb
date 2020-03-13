using MediatR;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Queries.GetUserInfo
{
    public class GetUserInfoQuery : IRequest<UserInfoVm>
    {
        public ClaimsPrincipal User { get; set; }

        public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoVm>
        {
            public Task<UserInfoVm> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
            {
                var userInfo = new UserInfoVm
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
}
