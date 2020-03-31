using AutoMapper;
using BTB.Application.Authorize.Common;
using BTB.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Queries.GetAuthorizationInfo
{
    public class GetAuthorizationInfoQueryHandler : IRequestHandler<GetAuthorizationInfoQuery, AuthorizationInfoDto>
    {
        private readonly IMapper _mapper;

        public GetAuthorizationInfoQueryHandler(IMapper _mapper)
        {
            this._mapper = _mapper;
        }

        public Task<AuthorizationInfoDto> Handle(GetAuthorizationInfoQuery request, CancellationToken cancellationToken)
        {
            var authInfo = new AuthorizationInfo
            {
                IsAuthenticated = request.User.Identity.IsAuthenticated,
                UserName = request.User.Identity.Name,
                ExposedClaims = request.User.Claims
                    .ToDictionary(c => c.Type, c => c.Value)
            };

            var authInfoDto = _mapper.Map<AuthorizationInfoDto>(authInfo);

            return Task.FromResult(authInfoDto);
        }
    }
}