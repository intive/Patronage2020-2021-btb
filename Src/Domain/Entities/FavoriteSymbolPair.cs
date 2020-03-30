namespace BTB.Domain.Entities
{
    public class FavoriteSymbolPair
    {
        public string ApplicationUserId { get; set; }
        public int SymbolPairId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public SymbolPair SymbolPair { get; set; }
    }
}
