using BTB.Application.Common.Interfaces;
using BTB.Common;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
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
        public DbSet<FavoriteSymbolPair> FavoriteSymbolPairs { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<LogEntry> Logs { get; set; }
        public DbSet<GamblePoint> GamblePoints { get; set; }

        private readonly IUserAccessor _userAccessor;
        private readonly IDateTime _dateTime;

        public BTBDbContext(DbContextOptions<BTBDbContext> options)
         : base(options)
        {
        }

        public BTBDbContext(
            DbContextOptions<BTBDbContext> options,
            IUserAccessor userAccessor,
            IDateTime dateTime)
           : base(options)
        {
            _userAccessor = userAccessor;
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
            builder.ApplyConfiguration(new FavoriteSymbolPairConfiguration());
            builder.ApplyConfiguration(new LogEntryConfiguration());
            builder.ApplyConfiguration(new GamblePointConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _userAccessor.GetCurrentUserId();
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _userAccessor.GetCurrentUserId();
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }

                CreateAudit(entry);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private void CreateAudit(EntityEntry<AuditableEntity> entry)
        {
            var date = _dateTime.Now;
            var userId = _userAccessor.GetCurrentUserId();

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

                AuditTrails.Add(auditEntry);
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
