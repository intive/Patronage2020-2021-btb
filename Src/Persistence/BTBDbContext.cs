using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Common;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
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
        public DbSet<AuditTrail> AuditTrails { get; set; }

        private readonly ICurrentUserIdentityService _currentUserService;
        private readonly IDateTime _dateTime;

        public BTBDbContext(DbContextOptions<BTBDbContext> options)
         : base(options)
        {
        }

        public BTBDbContext(
            DbContextOptions<BTBDbContext> options,
            ICurrentUserIdentityService currentUserIdentity,
            IDateTime dateTime)
           : base(options)
        {
            _currentUserService = currentUserIdentity;
            _dateTime = dateTime;
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
            builder.ApplyConfiguration(new AuditTrailConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }

                this.CreateAudit(entry);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private void CreateAudit(EntityEntry<AuditableEntity> entry)
        {
            var date = _dateTime.Now;
            var userId = _currentUserService.UserId;

            foreach (var property in entry.Properties)
            {
                if (!property.IsModified)
                    continue;

                var auditEntry = new AuditTrail
                {
                    Table = entry.Entity.GetType().Name,
                    Column = property.Metadata.Name,
                    UserId = userId,
                    OldValue = $"{property.OriginalValue}",
                    NewValue = $"{property.CurrentValue}",
                    Date = date
                };

                this.AuditTrails.Add(auditEntry);
            }
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
