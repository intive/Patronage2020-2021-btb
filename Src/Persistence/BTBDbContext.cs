using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BTB.Persistence
{
    public class BTBDbContext : IdentityDbContext, IBTBDbContext
    {
        public DbSet<UserProfileInfo> UserProfileInfo { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<SymbolPair> SymbolPairs { get; set; }
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
            builder.ApplyConfiguration(new SymbolPairConfiguration());
            builder.ApplyConfiguration(new KlineConfiguration());
        }

        public async Task<int> ClearKlinesAsync(DateTime startDate, DateTime stopDate, TimestampInterval klineType)
        {
            long fromDateTimestamp = ((DateTimeOffset)startDate).ToUnixTimeSeconds();
            long toDateTimestamp = ((DateTimeOffset)stopDate).ToUnixTimeSeconds();

            string filter = klineType == TimestampInterval.Zero ?
                string.Empty
                    :
                " AND DurationTimestamp = " + ((long)klineType).ToString();

            string query = "DELETE FROM Klines WHERE OpenTimestamp > " + fromDateTimestamp.ToString() +
                " AND OpenTimestamp < " + toDateTimestamp.ToString() + filter;

            return await Database.ExecuteSqlRawAsync(query);
        }
    }
}