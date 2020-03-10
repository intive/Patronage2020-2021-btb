using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BTB.Persistence
{
    public class BTBDbContext : DbContext, IBTBDbContext
    {
        public DbSet<UserProfileInfo> UserProfileInfo { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public BTBDbContext(DbContextOptions<BTBDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserProfileInfoConfiguration());
            builder.ApplyConfiguration(new AlertConfiguration());
        }
    }
}
