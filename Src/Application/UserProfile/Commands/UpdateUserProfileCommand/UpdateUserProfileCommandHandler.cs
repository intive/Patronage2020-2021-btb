using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.UserProfile.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBTBBinanceClient _client;
        private readonly ICurrentUserIdentityService _userIdentity;

        public UpdateUserProfileCommandHandler(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, ICurrentUserIdentityService userIdentity)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _userIdentity = userIdentity;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.FavouriteTradingPair))
            {
                if (_client.GetSymbolNames(request.FavouriteTradingPair) == null)
                {
                    throw new BadRequestException($"Trading pair symbol '{request.FavouriteTradingPair}' does not exist.");
                }
            }

            string userId = _userIdentity.UserId;
            UserProfileInfo dbUserProfileInfo = await _context.UserProfileInfo.SingleOrDefaultAsync(i => i.UserId == userId, cancellationToken);

            if (dbUserProfileInfo == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request, dbUserProfileInfo);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
