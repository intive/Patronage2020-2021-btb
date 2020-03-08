using AutoMapper;
using Binance.Net;
using Binance.Net.Interfaces;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.UserProfile.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileCommand : IRequest
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }

        public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBinanceClient _client;

            public CreateUserProfileCommandHandler(IBTBDbContext context, IMapper mapper, IBinanceClient client)
            {
                _context = context;
                _mapper = mapper;
                _client = client;
            }

            public async Task<Unit> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
            {
                var binanceResponse = await _client.GetPriceAsync(request.FavouriteTradingPair);
                if (!binanceResponse.Success)
                {
                    throw new BadRequestException($"Trading pair symbol \"{request.FavouriteTradingPair}\" does not exist.");
                }

                var dbUserProfileInfo = await _context.UserProfileInfo.Where(i => i.UserId == request.UserId).SingleOrDefaultAsync();

                if (dbUserProfileInfo != null)
                {
                    throw new BadRequestException($"User (ID: {request.UserId}) has already created a profile.");
                }

                var newUserProfileInfo = _mapper.Map<UserProfileInfo>(request);
                await _context.UserProfileInfo.AddAsync(newUserProfileInfo);
                await _context.SaveChangesAsync(CancellationToken.None);

                return Unit.Value;
            }
        }
    }
}
