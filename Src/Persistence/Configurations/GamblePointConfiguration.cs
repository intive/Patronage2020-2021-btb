using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class GamblePointConfiguration : IEntityTypeConfiguration<GamblePoint>
    {
        public void Configure(EntityTypeBuilder<GamblePoint> builder)
        {
            builder.HasKey(p => new { p.UserId });

            builder.Property(p => p.FreePoints)
                .IsRequired()
                .HasColumnType("decimal(18, 9)");

            builder.Property(p => p.SealedPoints)
                .IsRequired()
                .HasColumnType("decimal(18, 9)");

            builder.HasOne(p => p.User)
                .WithOne(u => u.GamblePoints)
                .HasForeignKey<GamblePoint>(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
