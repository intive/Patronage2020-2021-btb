using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.UserProfile.Common;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.UserProfile.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfileInfoVm>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBTBBinanceClient _client;
        private readonly IUserAccessor _userAccessor;

        public CreateUserProfileCommandHandler(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _userAccessor = userAccessor;
        }

        public async Task<UserProfileInfoVm> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.FavouriteTradingPair))
            {
                if (_client.GetSymbolNames(request.FavouriteTradingPair) == null)
                {
                    throw new BadRequestException($"Trading pair symbol '{request.FavouriteTradingPair}' does not exist.");
                }
            }

            string userId = _userAccessor.GetCurrentUserId();
            UserProfileInfo dbUserProfileInfo = await _context.UserProfileInfo.SingleOrDefaultAsync(i => i.UserId == userId, cancellationToken);

            if (dbUserProfileInfo != null)
            {
                throw new BadRequestException($"User has already created a profile.");
            }

            var newUserProfileInfo = _mapper.Map<UserProfileInfo>(request);
            newUserProfileInfo.UserId = userId;
            await _context.UserProfileInfo.AddAsync(newUserProfileInfo, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserProfileInfoVm>(newUserProfileInfo);
        }
    }
}
