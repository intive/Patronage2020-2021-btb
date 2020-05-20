using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Persistence.Configurations
{
    public class AlertMessageTemplateConfiguration : IEntityTypeConfiguration<AlertMessageTemplate>
    {
        public void Configure(EntityTypeBuilder<AlertMessageTemplate> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Type)
                .IsRequired();

            builder.Property(t => t.Message)
                .IsRequired();
        }
    }
}
