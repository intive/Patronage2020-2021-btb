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
        private readonly IAlertConditionDetector<CrossingConditionDetectorParameters> _crossingConditionDetector;
        private TimestampInterval _klineInterval;

        private static readonly IDictionary<int, int> _notificationTriggeredFlags;

        static SendEmailNotificationsCommandHandler()
        {
            _notificationTriggeredFlags = new Dictionary<int, int>();
        }

        public SendEmailNotificationsCommandHandler(IBTBDbContext context, IEmailService emailService,
            IAlertConditionDetector<CrossingConditionDetectorParameters> crossingConditionDetector)
        {
            _context = context;
            _emailService = emailService;
            _crossingConditionDetector = crossingConditionDetector;
        }

        public async Task<Unit> Handle(SendEmailNotificationsCommand request, CancellationToken cancellationToken)
        {
            _klineInterval = request.KlineInterval;

            foreach (var alert in _context.Alerts)
            {
                if (!alert.SendEmail)
                {
                    continue;
                }

                if (alert.TriggerOnce && alert.WasTriggered)
                {
                    continue;
                }

                if (await AreConditionsMet(alert, cancellationToken))
                {
                    await SendEmailMessageAsync(alert);
                }
            }

            return Unit.Value;
        }
        private async Task<bool> AreConditionsMet(Alert alert, CancellationToken cancellationToken)
        {
            Kline lastKline = await GetLastKlineBySymbolPairIdAsync(alert.SymbolPairId);
            if (lastKline == null)
            {
                return false;
            }

            if (WasNotificationTriggeredByKline(alert.Id, lastKline.Id))
            {
                return false;
            }

            var crossingParameters = new CrossingConditionDetectorParameters()
            {
                Kline = lastKline
            };

            if (!_crossingConditionDetector.IsConditionMet(alert, crossingParameters))
            {
                return false;
            }

            alert.WasTriggered = true;
            _context.Alerts.Update(alert);
            await _context.SaveChangesAsync(cancellationToken);

            SetNofiticationTriggeredFlag(alert.Id, lastKline.Id);
            return true;
        }

        private void SetNofiticationTriggeredFlag(int alertId, int klineId)
        {
            if (_notificationTriggeredFlags.ContainsKey(alertId))
            {
                _notificationTriggeredFlags[alertId] = klineId;
            }
            else
            {
                _notificationTriggeredFlags.Add(alertId, klineId);
            }
        }

        private bool WasNotificationTriggeredByKline(int alertId, int klineId)
        {
            if (_notificationTriggeredFlags.ContainsKey(alertId))
            {
                if (_notificationTriggeredFlags[alertId] == klineId)
                {
                    return true;
                }
            }

            return false;
        }

        private Task<Kline> GetLastKlineBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync(kline => kline.SymbolPairId == symbolPairId);
        }

        private async Task SendEmailMessageAsync(Alert alert)
        {
            EmailTemplate template = await _context.EmailTemplates.SingleOrDefaultAsync();
            _emailService.Send(alert.Email, "BTB trading pair alert", alert.Message, template);
        }

        public static void ResetTriggerFlags()
        {
            foreach (var key in _notificationTriggeredFlags.Keys)
            {
                _notificationTriggeredFlags.Remove(key);
            }
        }
    }
}
