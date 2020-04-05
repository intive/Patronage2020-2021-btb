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

namespace BTB.Application.System.Commands.Alerts.SendEmailNotificationsCommand
{
    public class SendEmailNotificationsCommandHandler : IRequestHandler<SendEmailNotificationsCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IEmailService _emailService;

        private static readonly IDictionary<int, Kline> _pairIdToLastKlineMap = new Dictionary<int, Kline>();
        private static bool _arePairsLoaded = false;

        public SendEmailNotificationsCommandHandler(IBTBDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendEmailNotificationsCommand request, CancellationToken cancellationToken)
        {
            if (!_arePairsLoaded)
            {
                await LoadPairsToDictionaryAsync();
                _arePairsLoaded = true;
            }
            else
            {
                await SendNotificationsAsync();
            }

            return Unit.Value;
        }

        private async Task LoadPairsToDictionaryAsync()
        {
            foreach (var pair in _context.SymbolPairs)
            {
                Kline kline = await GetLastKlineBySymbolPairIdAsync(pair.Id);
                _pairIdToLastKlineMap.Add(pair.Id, kline);
            }
        }

        private async Task SendNotificationsAsync()
        {
            foreach (var alert in _context.Alerts)
            {
                if (!alert.SendEmail)
                {
                    continue;
                }

                if (await AreConditionsMet(alert))
                {
                    _emailService.Send(alert.Email, "BTB trading pair alert", alert.Message);
                }
            }
        }

        private async Task<bool> AreConditionsMet(Alert alert)
        {
            bool areConditionsMet = false;

            switch (alert.Condition)
            {
                case AlertCondition.Crossing:
                    Kline lastDbKline = await GetLastKlineBySymbolPairIdAsync(alert.SymbolPairId);
                    Kline lastCachedKline = _pairIdToLastKlineMap[alert.SymbolPairId];

                    areConditionsMet = alert.ValueType switch
                    {
                        AlertValueType.Volume => Crossed(lastDbKline.Volume, lastCachedKline.Volume, alert.Value),
                        AlertValueType.Price => Crossed(lastDbKline.ClosePrice, lastCachedKline.ClosePrice, alert.Value),
                        AlertValueType.TradeCount => throw new NotImplementedException()
                    };
                    
                    _pairIdToLastKlineMap[alert.SymbolPairId] = lastDbKline;
                    break;
            }

            return areConditionsMet;
        }

        private bool Crossed(decimal newValue, decimal oldvalue, decimal threshold)
        {
            if (newValue > threshold && threshold > oldvalue || newValue < threshold && threshold < oldvalue)
            {
                return true;
            }

            return false;
        }

        private Task<Kline> GetLastKlineBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .Where(kline => kline.SymbolPairId == symbolPairId)
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync();
        }
    }
}
