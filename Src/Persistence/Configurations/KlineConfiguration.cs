using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class KlineConfiguration : IEntityTypeConfiguration<Kline>
    {
        public void Configure(EntityTypeBuilder<Kline> builder)
        {
            builder.HasOne(a => a.BuySymbol)
                .WithMany(a => a.KlinesAsBuy)
                .HasForeignKey(a => a.BuySymbolId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.SellSymbol)
                .WithMany(a => a.KlinesAsSell)
                .HasForeignKey(a => a.SellSymbolId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
