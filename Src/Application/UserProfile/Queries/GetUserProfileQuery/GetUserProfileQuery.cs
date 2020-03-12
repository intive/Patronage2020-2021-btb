using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
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
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetUserProfileQueryHandler(IBTBDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<UserProfileInfoVm> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userProfileInfo = await _context.UserProfileInfo.Where(i => i.UserId == userId).SingleOrDefaultAsync();

                if (userProfileInfo == null)
                {
                    throw new NotFoundException();
                }

                return _mapper.Map<UserProfileInfoVm>(userProfileInfo);
            }
        }
    }
}