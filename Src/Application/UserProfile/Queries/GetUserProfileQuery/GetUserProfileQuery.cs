using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.UserProfile.Queries.GetUserProfileQuery
{
    public class GetUserProfileQuery : IRequest<UserProfileInfoDto>
    {
        public int UserId { get; set; }

        public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileInfoDto>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;

            public GetUserProfileQueryHandler(IBTBDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserProfileInfoDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
            {
                var userProfileInfo = await _context.UserProfileInfo.Where(i => i.UserId == request.UserId).SingleOrDefaultAsync();

                if (userProfileInfo == null)
                {
                    throw new NotFoundException();
                }

                return _mapper.Map<UserProfileInfoDto>(userProfileInfo);
            }
        }
    }
}
