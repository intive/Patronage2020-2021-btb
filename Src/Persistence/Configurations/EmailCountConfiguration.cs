using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class EmailCountConfiguration : IEntityTypeConfiguration<EmailCount>
    {
        public void Configure(EntityTypeBuilder<EmailCount> builder)
        {
            builder.HasIndex(e => e.Id)
                .IsUnique();

            builder.Property(e => e.DailyCount)
                .IsRequired();
        }
    }
}
