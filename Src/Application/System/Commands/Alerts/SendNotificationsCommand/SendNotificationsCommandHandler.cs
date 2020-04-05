using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.Alerts.SendNotificationsCommand
{
    public class SendNotificationsCommandHandler : IRequestHandler<SendNotificationsCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IEmailService _emailService;
        private static readonly IDictionary<int, Kline> _pairIdToLastKlineMap = new Dictionary<int, Kline>();
        private static bool _arePairsLoaded = false;

        public SendNotificationsCommandHandler(IBTBDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendNotificationsCommand request, CancellationToken cancellationToken)
        {
            if (!_arePairsLoaded)
            {
                await LoadPairsToDictionaryAsync();
                _arePairsLoaded = true;
            }
            else
            {
                await CheckAlertsAsync();
            }

            return Unit.Value;
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

                if (alert.ValueType == AlertValueType.Volume)
                {

                    if (alert.Condition == AlertCondition.Crossing)
                    {

                        if (lastDbKline.Volume > alert.Value && alert.Value > lastCachedKline.Volume ||
                            lastDbKline.Volume < alert.Value && alert.Value < lastCachedKline.Volume)
                        {
                            _emailService.Send(alert.Email, "BTB trading pair alert", alert.Message);
                        }
                    }
                }

                _pairIdToLastKlineMap[alert.SymbolPairId] = lastDbKline;
            }
        }

        private async Task<Kline> GetLastKlineBySymbolPairId(int symbolPairId)
        {
            return await _context.Klines
                .Where(kline => kline.SymbolPairId == symbolPairId)
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync();
        }
    }
}
