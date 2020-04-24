using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Persistence.Configurations
{
    public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.HasKey(a => a.Id)
                .IsClustered(false);

            builder.HasIndex(k => k.TimeStampUtc)
                .IsUnique(false)
                .IsClustered(true);

            builder.HasIndex(k => k.Category)
                .IsUnique(false)
                .IsClustered(false);

            builder.HasIndex(k => k.Level)
                .IsUnique(false) 
                .IsClustered(false);
        }
    }
}
