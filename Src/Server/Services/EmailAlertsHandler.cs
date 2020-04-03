using Binance.Net;
using Binance.Net.Interfaces;
using BTB.Application.Common.Interfaces;
using BTB.Application.System.Commands.Alerts.SendEmailCommand;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using BTB.Server.Common;
using BTB.Server.Common.CronGeneric;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class EmailAlertsHandler
    {
        private readonly IBTBDbContext _context;
        private readonly IEmailService _emailService = new EmailService(); // this is temporary, didnt know how to inject this
        
        private static readonly IDictionary<int, Kline> _pairIdToLastKlineMap = new Dictionary<int, Kline>(); // must be static for now
        private static bool _arePairsLoaded = false;

        public EmailAlertsHandler(IBTBDbContext context)
        {
            _context = context;
        }

        public async Task Handle()
        {        
            if (!_arePairsLoaded)
            {
                await LoadPairsToDictionaryAsync();
                _arePairsLoaded = true;
                await CheckAlertsAsync();
            }
            else
            {
                await CheckAlertsAsync();
            }
        }

        private async Task LoadPairsToDictionaryAsync()
        {
            foreach (var pair in _context.SymbolPairs)
            {
                Kline kline = await GetLastKlineBySymbolPairId(pair.Id);
                _pairIdToLastKlineMap.Add(pair.Id, kline);
            }
        }

        private async Task CheckAlertsAsync()
        {
            foreach (var alert in _context.Alerts)
            {
                if (!alert.SendEmail)
                {
                    continue;
                }

                Kline lastDbKline = await GetLastKlineBySymbolPairId(alert.SymbolPairId);
                Kline lastCachedKline = _pairIdToLastKlineMap[alert.SymbolPairId];

                // value types switch
                if (alert.ValueType == AlertValueType.Volume)
                {
                    // conditions switch
                    if (alert.Condition == AlertCondition.Crossing)
                    {
                        // actual condition detection
                        if (lastDbKline.Volume > alert.Value && alert.Value > lastCachedKline.Volume ||
                            lastDbKline.Volume < alert.Value && alert.Value < lastCachedKline.Volume)
                        {
                            var handler = new SendEmailCommandHandler(_emailService);
                            await handler.Handle(new SendEmailCommand()
                            {
                                To = alert.Email,
                                EmailTitle = "BTB trading pair alert",
                                EmailContent = alert.Message
                            }, CancellationToken.None);
                        }
                    }
                }

                _pairIdToLastKlineMap[alert.SymbolPairId] = lastDbKline;
            }
        }

        // probably a method to be added to BinanceMiddleService
        private async Task<Kline> GetLastKlineBySymbolPairId(int symbolPairId)
        {
            return await _context.Klines
                .Where(kline => kline.SymbolPairId == symbolPairId)
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync();
        }
    }
}
