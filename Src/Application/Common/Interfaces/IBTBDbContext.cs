using Binance.Net.Objects;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IBTBDbContext
    {
        DbSet<UserProfileInfo> UserProfileInfo { get; set; }
        DbSet<Alert> Alerts { get; set; }
        DbSet<Symbol> Symbols { get; set; }
        DbSet<SymbolPair> SymbolPairs { get; set; }
        DbSet<Kline> Klines { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> ClearKlinesAsync(DateTime startDate, DateTime stopTime, TimestampInterval klineType);
    }
}