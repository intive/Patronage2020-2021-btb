using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBTBDbContext
    {
        DbSet<UserProfileInfo> UserProfileInfo { get; set; }
        DbSet<Alert> Alerts { get; set; }
        DbSet<Symbol> Symbols { get; set; }
        DbSet<Kline> Klines { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}