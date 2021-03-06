﻿using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair
{
    public class CreateFavoriteSymbolPairHandler : IRequestHandler<CreateFavoriteSymbolPairCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public CreateFavoriteSymbolPairHandler(IBTBDbContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateFavoriteSymbolPairCommand request, CancellationToken cancellationToken)
        {
            var favoritePair = _mapper.Map<FavoriteSymbolPair>(request);
            favoritePair.ApplicationUserId = _userAccessor.GetCurrentUserId();

            var dbFavoritePair = await _context.FavoriteSymbolPairs.FindAsync(_userAccessor.GetCurrentUserId(), request.SymbolPairId);
            if(dbFavoritePair != null)
            {
                throw new BadRequestException($"User (id : {_userAccessor.GetCurrentUserId()}) has already added pair with id : {request.SymbolPairId} to favorites.");
            }

            var dbSymbolPair = await _context.SymbolPairs.FindAsync(request.SymbolPairId);
            if(dbSymbolPair == null)
            {
                throw new BadRequestException($"Pair with id : {request.SymbolPairId} does not exist.");
            }

            await _context.FavoriteSymbolPairs.AddAsync(favoritePair, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
