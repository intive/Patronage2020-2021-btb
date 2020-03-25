using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class KlineConfiguration : IEntityTypeConfiguration<Kline>
    {
        public void Configure(EntityTypeBuilder<Kline> builder)
        {
            builder.HasOne(a => a.SymbolPair)
                .WithMany(a => a.Klines)
                .HasForeignKey(a => a.SymbolPairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(a => a.OpenPrice)
                .HasColumnType("decimal(16, 5)");

            builder.Property(a => a.ClosePrice)
                .HasColumnType("decimal(16, 5)");

            builder.Property(a => a.HighestPrice)
                .HasColumnType("decimal(16, 5)");

            builder.Property(a => a.LowestPrice)
                .HasColumnType("decimal(16, 5)");

            builder.Property(a => a.Volume)
                .HasColumnType("decimal(16, 8)");

            builder.ToTable("Klines");
        }
    }
}
