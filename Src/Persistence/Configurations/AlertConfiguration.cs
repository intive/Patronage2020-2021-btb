using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Symbol)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(a => a.Condition)
                .IsRequired();

            builder.Property(a => a.ValueType)
                .IsRequired();

            builder.Property(a => a.Value)
                .IsRequired();

            builder.Property(a => a.UserId)
                .IsRequired();

            builder.Property(a => a.SendEmail)
                .IsRequired();

            builder.Property(a => a.Email)
                .IsRequired(false);

            builder.Property(a => a.Message)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(a => a.User)
                .WithMany(u => u.Alerts)
                .HasForeignKey(a => a.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
