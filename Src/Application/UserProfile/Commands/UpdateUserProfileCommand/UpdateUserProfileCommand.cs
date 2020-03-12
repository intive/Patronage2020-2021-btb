﻿using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.UserProfile.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommand : IRequest
    {
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }

        public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBinanceClient _client;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UpdateUserProfileCommandHandler(IBTBDbContext context, IMapper mapper, IBinanceClient client, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _client = client;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
            {
                var binanceResponse = await _client.GetPriceAsync(request.FavouriteTradingPair);
                if (!binanceResponse.Success)
                {
                    throw new BadRequestException($"Trading pair symbol \"{request.FavouriteTradingPair}\" does not exist.");
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var dbUserProfileInfo = await _context.UserProfileInfo.Where(i => i.UserId == userId).SingleOrDefaultAsync();

                if (dbUserProfileInfo == null)
                {
                    throw new NotFoundException();
                }

                _mapper.Map(request, dbUserProfileInfo);
                await _context.SaveChangesAsync(CancellationToken.None);
                return Unit.Value;
            }
        }
    }
}