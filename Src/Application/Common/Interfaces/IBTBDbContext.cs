using BTB.Domain.Entities;
using BTB.Domain.Example.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBTBDbContext
    {
        DbSet<ExampleEntity> ExampleEntities { get; set; }
        DbSet<UserProfileInfo> UserProfileInfo { get; set; }
        DbSet<Alert> Alerts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
