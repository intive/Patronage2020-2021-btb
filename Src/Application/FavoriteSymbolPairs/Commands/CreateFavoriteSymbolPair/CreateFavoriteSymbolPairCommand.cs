using MediatR;

namespace BTB.Application.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair
{
    public class CreateFavoriteSymbolPairCommand : IRequest
    {
        public int SymbolPairId { get; set; }
    }
}
