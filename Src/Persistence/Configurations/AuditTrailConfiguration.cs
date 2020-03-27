using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.ToTable("AuditTrail");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Table)
                .IsRequired();

            builder.Property(a => a.Column)
                .IsRequired();

            builder.Property(a => a.UserId)
                .IsRequired(false);

            builder.Property(a => a.OldValue)
                .IsRequired(false);

            builder.Property(a => a.NewValue)
                .IsRequired();

            builder.Property(a => a.Date)
                .IsRequired();
        }
    }
}