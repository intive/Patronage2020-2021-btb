using BTB.Application.Common.Interfaces;
using BTB.Domain.Example.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Persistence
{
    public class BTBDbContext : DbContext, IBTBDbContext
    {
        public DbSet<ExampleEntity> ExampleEntities { get; set; }

        public BTBDbContext(DbContextOptions<BTBDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
