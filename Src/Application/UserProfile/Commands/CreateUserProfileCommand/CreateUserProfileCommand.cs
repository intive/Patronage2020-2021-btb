using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.UserProfile.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileCommand : IRequest
    {
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }

        public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBinanceClient _client;
            private readonly ICurrentUserIdentityService _userIdentity;

            public CreateUserProfileCommandHandler(IBTBDbContext context, IMapper mapper, IBinanceClient client, ICurrentUserIdentityService userIdentity)
            {
                _context = context;
                _mapper = mapper;
                _client = client;
                _userIdentity = userIdentity;
            }

            public async Task<Unit> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
            {
                var binanceResponse = await _client.GetPriceAsync(request.FavouriteTradingPair, cancellationToken);
                if (!binanceResponse.Success)
                {
                    throw new BadRequestException($"Trading pair symbol '{request.FavouriteTradingPair}' does not exist.");
                }

                var userId = _userIdentity.UserId;
                var dbUserProfileInfo = await _context.UserProfileInfo.SingleOrDefaultAsync(i => i.UserId == userId, cancellationToken);

                if (dbUserProfileInfo != null)
                {
                    throw new BadRequestException($"User has already created a profile.");
                }

                var newUserProfileInfo = _mapper.Map<UserProfileInfo>(request);
                newUserProfileInfo.UserId = userId;
                await _context.UserProfileInfo.AddAsync(newUserProfileInfo, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}