using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Persistence.Configurations
{
    class UserProfileInfoConfiguration : IEntityTypeConfiguration<UserProfileInfo>
    {
        public void Configure(EntityTypeBuilder<UserProfileInfo> builder)
        {
            builder.ToTable("UserProfileInfo");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(i => i.UserId)
                .IsRequired();
            builder.HasIndex(i => i.UserId)
                .IsUnique();

            builder.Property(i => i.Username)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(i => i.ProfileBio)
                .HasMaxLength(256)
                .IsRequired(false);

            builder.Property(i => i.FavouriteTradingPair)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.HasOne(i => i.User)
                .WithOne(u => u.ProfileInfo)
                .HasForeignKey<UserProfileInfo>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
