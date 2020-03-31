using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.UserProfile.Common;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.UserProfile.Queries.GetUserProfileQuery
{
    public class GetUserProfileQuery : IRequest<UserProfileInfoVm>
    {
        public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileInfoVm>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserIdentityService _userIdentity;

            public GetUserProfileQueryHandler(IBTBDbContext context, IMapper mapper, ICurrentUserIdentityService userIdentity)
            {
                _context = context;
                _mapper = mapper;
                _userIdentity = userIdentity;
            }

            public async Task<UserProfileInfoVm> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
            {
                string userId = _userIdentity.UserId;
                UserProfileInfo userProfileInfo = await _context.UserProfileInfo.SingleOrDefaultAsync(i => i.UserId == userId, cancellationToken);

                if (userProfileInfo == null)
                {
                    throw new NotFoundException();
                }

                return _mapper.Map<UserProfileInfoVm>(userProfileInfo);
            }
        }
    }
}