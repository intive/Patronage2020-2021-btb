using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.FavoriteSymbolPairs.Commands.DeleteFavoriteSymbolPair
{
    public class DeleteFavoriteSymbolPairHandler : IRequestHandler<DeleteFavoriteSymbolPairCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public DeleteFavoriteSymbolPairHandler(IBTBDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(DeleteFavoriteSymbolPairCommand request, CancellationToken cancellationToken)
        {
            var dbFavoriteSymbolPair = _context.FavoriteSymbolPairs.Find(_userAccessor.GetCurrentUserId(), request.SymbolPairId);
            if(dbFavoriteSymbolPair == null)
            {
                throw new NotFoundException($"User (id: {_userAccessor.GetCurrentUserId()}) has no favorite symbol pair with id {request.SymbolPairId}."); ;
            }

            _context.FavoriteSymbolPairs.Remove(dbFavoriteSymbolPair);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
