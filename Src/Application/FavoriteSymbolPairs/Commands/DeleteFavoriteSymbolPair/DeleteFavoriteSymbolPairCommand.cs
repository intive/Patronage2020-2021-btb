using MediatR;

namespace BTB.Application.FavoriteSymbolPairs.Commands.DeleteFavoriteSymbolPair
{
    public class DeleteFavoriteSymbolPairCommand : IRequest
    {
        public int SymbolPairId { get; set; }
    }
}
