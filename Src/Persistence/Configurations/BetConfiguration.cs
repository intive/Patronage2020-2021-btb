using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Persistence.Configurations
{
    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Points)
                .IsRequired()
                .HasColumnType("decimal(18, 9)");

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.RateType)
                .IsRequired();

            builder.Property(b => b.TimeInterval)
                .IsRequired();

            builder.Property(b => b.KlineOpenTimestamp)
                .IsRequired();

            builder.Property(b => b.IsActive)
                .IsRequired();

            builder.HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.SymbolPair)
                .WithMany(s => s.Bets)
                .HasForeignKey(b => b.SymbolPairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
