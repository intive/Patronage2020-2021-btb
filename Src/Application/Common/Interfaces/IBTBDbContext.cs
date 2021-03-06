﻿using BTB.Domain.Common;
using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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
        DbSet<AuditTrail> AuditTrails { get; set; }
        DbSet<FavoriteSymbolPair> FavoriteSymbolPairs { get; set; }
        DbSet<EmailTemplate> EmailTemplates { get; set; }
        DbSet<LogEntry> Logs { get; set; }
        DbSet<GamblePoint> GamblePoints { get; set; }
        DbSet<Bet> Bets { get; set; }
        DbSet<EmailCount> EmailCounts { get; set; }
        DbSet<AlertMessageTemplate> AlertMessageTemplates { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> ClearKlinesAsync(DateTime startDate, DateTime stopTime, TimestampInterval klineType);
    }
}
