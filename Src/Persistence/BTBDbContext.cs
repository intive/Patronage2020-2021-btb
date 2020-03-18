using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTB.Persistence
{
    public class BTBDbContext : IdentityDbContext, IBTBDbContext
    {
        public DbSet<UserProfileInfo> UserProfileInfo { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Kline> Klines { get; set; }

        public BTBDbContext(DbContextOptions<BTBDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserProfileInfoConfiguration());
            builder.ApplyConfiguration(new AlertConfiguration());
            builder.ApplyConfiguration(new SymbolConfiguration());
            builder.ApplyConfiguration(new KlineConfiguration());
        }
    }
}