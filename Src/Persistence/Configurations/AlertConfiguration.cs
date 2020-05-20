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

            builder.Property(a => a.Condition)
                .IsRequired();

            builder.Property(a => a.ValueType)
                .IsRequired();

            builder.Property(a => a.Value)
                .IsRequired()
                .HasColumnType("decimal(18, 9)");

            builder.Property(a => a.AdditionalValue)
                .IsRequired()
                .HasColumnType("decimal(18, 9)");

            builder.Property(a => a.UserId)
                .IsRequired();

            builder.Property(a => a.SymbolPairId)
                .IsRequired();

            builder.Property(a => a.SendEmail)
                .IsRequired();

            builder.Property(a => a.Email)
                .IsRequired(false);

            builder.Property(a => a.TriggerOnce)
                .IsRequired();

            builder.Property(a => a.WasTriggered)
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithMany(u => u.Alerts)
                .HasForeignKey(a => a.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.SymbolPair)
                .WithMany(s => s.Alerts)
                .HasForeignKey(a => a.SymbolPairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.MessageTemplate)
                .WithMany(t => t.Alerts)
                .HasForeignKey(a => a.MessageTemplateId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
