using BTB.Application.Common.Interfaces;
using BTB.Application.ConditionDetectors.Crossing;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.SendEmailNotificationsCommand
{
    public class SendEmailNotificationsCommandHandler : IRequestHandler<SendEmailNotificationsCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IEmailService _emailService;

        private static readonly IDictionary<int, Kline> _pairIdToLastKlineMap;
        private static bool _pairsNotLoaded;
        private static readonly IAlertConditionDetector<CrossingConditionDetectorParameters> _crosssingConditionDetector;

        private TimestampInterval _klineInterval;

        static SendEmailNotificationsCommandHandler()
        {
            _pairsNotLoaded = true;
            _pairIdToLastKlineMap = new Dictionary<int, Kline>();
            _crosssingConditionDetector = new CrossingConditionDetector();
        }

        public SendEmailNotificationsCommandHandler(IBTBDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendEmailNotificationsCommand request, CancellationToken cancellationToken)
        {
            _klineInterval = request.KlineInterval;

            if (_pairsNotLoaded)
            {
                await LoadPairsToDictionaryAsync();
                _pairsNotLoaded = false;
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
                    await SendEmailMessageAsync(alert);
                }
            }
        }

        private async Task<bool> AreConditionsMet(Alert alert)
        {
            Kline lastDbKline = await GetLastKlineBySymbolPairIdAsync(alert.SymbolPairId);
            Kline lastCachedKline = _pairIdToLastKlineMap[alert.SymbolPairId];
            _pairIdToLastKlineMap[alert.SymbolPairId] = lastDbKline;

            var crossingParameters = new CrossingConditionDetectorParameters()
            {
                NewKline = lastDbKline,
                OldKline = lastCachedKline
            };

            if (_crosssingConditionDetector.IsConditionMet(alert, crossingParameters))
            {
                return true;
            }

            return false;
        }

        private Task<Kline> GetLastKlineBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync(kline => kline.SymbolPairId == symbolPairId && kline.DurationTimestamp == _klineInterval);
        }

        private async Task SendEmailMessageAsync(Alert alert)
        {
            EmailTemplate template = await _context.EmailTemplates.SingleOrDefaultAsync();
            _emailService.Send(alert.Email, "BTB trading pair alert", alert.Message, template);
        }
    }
}
