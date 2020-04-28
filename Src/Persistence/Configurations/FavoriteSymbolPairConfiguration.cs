using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class FavoriteSymbolPairConfiguration : IEntityTypeConfiguration<FavoriteSymbolPair>
    {
        public void Configure(EntityTypeBuilder<FavoriteSymbolPair> builder)
        {
            builder.HasKey(f => new { f.ApplicationUserId, f.SymbolPairId });

            builder.HasOne(f => f.SymbolPair)
                .WithMany(s => s.FavoritePairs)
                .HasForeignKey(f => f.SymbolPairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.ApplicationUser)
                .WithMany(u => u.FavoritePairs)
                .HasForeignKey(f => f.ApplicationUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
