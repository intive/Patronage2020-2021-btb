﻿using BTB.Application.Common.Hubs;
using BTB.Application.Common.Interfaces;
using BTB.Application.ConditionDetectors;
using BTB.Application.ConditionDetectors.Between;
using BTB.Application.ConditionDetectors.Crossing;
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
        private readonly IBrowserNotificationHub _browserAlert;
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

        public SendEmailNotificationsCommandHandler(IBTBDbContext context, IEmailService emailService, IBrowserNotificationHub browserAlert, IEmailKeeper emailKeeper)
        {
            _context = context;
            _emailService = emailService;
            _emailKeeper = emailKeeper;
            _browserAlert = browserAlert;

            _crossingConditionDetector = new CrossingConditionDetector();
            _crossingUpConditionDetector = new CrossingUpConditionDetector();
            _crossingDownConditionDetector = new CrossingDownConditionDetector();
            _betweenConditionDetector = new BetweenConditionDetector();
        }

        public async Task<Unit> Handle(SendEmailNotificationsCommand request, CancellationToken cancellationToken)
        {
            _klineInterval = request.KlineInterval;

            var alerts = _context.Alerts;
            var triggeredAlerts = new List<Alert>();

            foreach (var alert in alerts)
            {
                if (alert.IsDisabled)
                {
                    continue;
                }

                if (!alert.SendEmail && !alert.SendInBrowser)
                {
                    continue;
                }

                if (await AreConditionsMet(alert, cancellationToken))
                {
                    if (alert.SendEmail)
                    {
                        await SendEmailMessageAsync(alert);
                    }

                    if (alert.SendInBrowser)
                    {
                        await SendInBrowserNotificationAsync(alert);
                    }
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
            Kline lastKline = await GetLastKlineBySymbolPairIdAsync(alert.SymbolPairId);
            if (lastKline == null)
            {
                return false;
            }

            if (WasNotificationTriggeredByKline(alert.Id, lastKline.Id))
            {
                return false;
            }

            var parameters = new BasicConditionDetectorParameters()
            {
                Kline = lastKline
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
                SetNofiticationTriggeredByKlineFlag(alert.Id, lastKline.Id);
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

        private Task<Kline> GetLastKlineBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync(kline => kline.SymbolPairId == symbolPairId && kline.DurationTimestamp == _klineInterval);
        }

        private async Task SendEmailMessageAsync(Alert alert)
        {
            if (!_emailKeeper.CheckIfLimitHasBeenReached())
            {
                EmailTemplate template = await _context.EmailTemplates.SingleOrDefaultAsync();
                _emailService.Send(alert.Email, "BTB trading pair alert", alert.Message, template);
            }
        }

        private async Task SendInBrowserNotificationAsync(Alert alert)
        {
            await _browserAlert.SendToUserAsync(alert.UserId, $"{alert.SymbolPair.PairName} is {alert.Condition} {alert.ValueType} {alert.Value}");
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
