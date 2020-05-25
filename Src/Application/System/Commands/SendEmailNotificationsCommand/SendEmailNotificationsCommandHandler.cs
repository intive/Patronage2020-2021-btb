using BTB.Application.Common.Interfaces;
using BTB.Application.ConditionDetectors;
using BTB.Application.ConditionDetectors.Between;
using BTB.Application.ConditionDetectors.Crossing;
using BTB.Application.System.Commands.SeedSampleData;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.SendEmailNotificationsCommand
{
    public class SendEmailNotificationsCommandHandler : IRequestHandler<SendEmailNotificationsCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IEmailKeeper _emailKeeper;
        private TimestampInterval _klineInterval;

        private readonly IAlertConditionDetector<BasicConditionDetectorParameters> _crossingConditionDetector;
        private readonly IAlertConditionDetector<BasicConditionDetectorParameters> _crossingUpConditionDetector;
        private readonly IAlertConditionDetector<BasicConditionDetectorParameters> _crossingDownConditionDetector;
        private readonly IAlertConditionDetector<BasicConditionDetectorParameters> _betweenConditionDetector;

        private static readonly IDictionary<int, int> _notificationTriggeredByKlineFlags;

        static SendEmailNotificationsCommandHandler()
        {
            _notificationTriggeredByKlineFlags = new Dictionary<int, int>();
        }

        public SendEmailNotificationsCommandHandler(IBTBDbContext context, IEmailService emailService, IEmailKeeper emailKeeper)
        {
            _context = context;
            _emailService = emailService;
            _emailKeeper = emailKeeper;

            _crossingConditionDetector = new CrossingConditionDetector();
            _crossingUpConditionDetector = new CrossingUpConditionDetector();
            _crossingDownConditionDetector = new CrossingDownConditionDetector();
            _betweenConditionDetector = new BetweenConditionDetector();
        }

        public async Task<Unit> Handle(SendEmailNotificationsCommand request, CancellationToken cancellationToken)
        {
            _klineInterval = request.KlineInterval;
            var alerts = _context.Alerts
                .Include(a => a.SymbolPair).ThenInclude(sp => sp.BuySymbol)
                .Include(a => a.SymbolPair).ThenInclude(sp => sp.SellSymbol);

            var triggeredAlerts = new List<Alert>();

            foreach (var alert in alerts)
            {
                if (!alert.SendEmail || alert.IsDisabled)
                {
                    continue;
                }

                if (await AreConditionsMet(alert, cancellationToken))
                {                 
                    alert.WasTriggered = true;
                    triggeredAlerts.Add(alert);           
                }
            }

            _context.Alerts.UpdateRange(triggeredAlerts);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var alert in triggeredAlerts)
            {
                await SendEmailMessageAsync(alert);
            }

            
            return Unit.Value;
        }

        private async Task<bool> AreConditionsMet(Alert alert, CancellationToken cancellationToken)
        {
            Kline kline = await GetLastClosedKlineBySymbolPairIdAsync(alert.SymbolPairId);
            if (kline == null)
            {
                return false;
            }

            if (WasNotificationTriggeredByKline(alert.Id, kline.Id))
            {
                return false;
            }

            var parameters = new BasicConditionDetectorParameters()
            {
                Kline = kline
            };

            bool isConditionMet = alert.Condition switch
            {
                AlertCondition.Crossing => _crossingConditionDetector.IsConditionMet(alert, parameters),
                AlertCondition.CrossingUp => _crossingUpConditionDetector.IsConditionMet(alert, parameters),
                AlertCondition.CrossingDown => _crossingDownConditionDetector.IsConditionMet(alert, parameters),
                AlertCondition.Between => _betweenConditionDetector.IsConditionMet(alert, parameters),
                _ => false
            };

            if (isConditionMet)
            {
                SetNofiticationTriggeredByKlineFlag(alert.Id, kline.Id);
            }

            return isConditionMet;
        }

        private void SetNofiticationTriggeredByKlineFlag(int alertId, int klineId)
        {
            if (_notificationTriggeredByKlineFlags.ContainsKey(alertId))
            {
                _notificationTriggeredByKlineFlags[alertId] = klineId;
            }
            else
            {
                _notificationTriggeredByKlineFlags.Add(alertId, klineId);
            }
        }

        private bool WasNotificationTriggeredByKline(int alertId, int klineId)
        {
            if (_notificationTriggeredByKlineFlags.ContainsKey(alertId))
            {
                if (_notificationTriggeredByKlineFlags[alertId] == klineId)
                {
                    return true;
                }
            }

            return false;
        }

        private Task<Kline> GetLastClosedKlineBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .OrderByDescending(kline => kline.OpenTimestamp)
                .Where(kline => kline.SymbolPairId == symbolPairId && kline.DurationTimestamp == _klineInterval)
                .Skip(1)
                .FirstOrDefaultAsync();
        }

        private async Task SendEmailMessageAsync(Alert alert)
        {
            if (!_emailKeeper.CheckIfLimitHasBeenReached())
            {
                EmailTemplate template = await _context.EmailTemplates.SingleOrDefaultAsync();
                AlertMessageTemplate messageTemplate = await _context.AlertMessageTemplates.SingleOrDefaultAsync(t => t.Type == alert.Condition);
                _emailService.Send(alert.Email, "BTB trading pair alert", AlertMessageTemplates.FillTemplate(alert, messageTemplate.Message), template);
            }
        }

        public static void ResetTriggerFlags()
        {
            foreach (var key in _notificationTriggeredByKlineFlags.Keys)
            {
                _notificationTriggeredByKlineFlags.Remove(key);
            }
        }
    }
}
