using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Persistence.Configurations
{
    public class SymbolPairConfiguration : IEntityTypeConfiguration<SymbolPair>
    {
        public void Configure(EntityTypeBuilder<SymbolPair> builder)
        {          

            builder.HasOne(a => a.BuySymbol)
                .WithMany(a => a.BuySymbolPairs)
                .HasForeignKey(a => a.BuySymbolId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.SellSymbol)
                .WithMany(a => a.SellSymbolPairs)
                .HasForeignKey(a => a.SellSymbolId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
