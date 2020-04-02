using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class KlineConfiguration : IEntityTypeConfiguration<Kline>
    {
        public void Configure(EntityTypeBuilder<Kline> builder)
        {
            builder.HasKey(a => a.Id)
                .IsClustered(false);

            builder.HasOne(a => a.SymbolPair)
                .WithMany(a => a.Klines)
                .HasForeignKey(a => a.SymbolPairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(k => k.DurationTimestamp)
                .IsUnique(false)
                .IsClustered(false);

            builder.HasIndex(k => k.SymbolPairId)
                .IsUnique(false)
                .IsClustered(true);

            builder.Property(a => a.OpenPrice)
                .HasColumnType("decimal(18, 9)");

            builder.Property(a => a.ClosePrice)
                .HasColumnType("decimal(18, 9)");

            builder.Property(a => a.HighestPrice)
                .HasColumnType("decimal(18, 9)");

            builder.Property(a => a.LowestPrice)
                .HasColumnType("decimal(18, 9)");

            builder.Property(a => a.Volume)
                .HasColumnType("decimal(18, 9)");

            builder.ToTable("Klines");
        }
    }
}
