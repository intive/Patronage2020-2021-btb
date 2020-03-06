using BTB.Domain.Example.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBTBDbContext
    {
        DbSet<ExampleEntity> ExampleEntities { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
