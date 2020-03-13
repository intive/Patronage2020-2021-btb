﻿using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BTB.Persistence.Configurations
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.Property(a => a.UserId)
                .IsRequired();

            builder.Property(a => a.Email)
                .IsRequired();
        }
    }
}