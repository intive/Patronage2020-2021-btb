using BTB.Application.Common.Hubs;
using BTB.Application.Common.Interfaces;
using BTB.Application.ConditionDetectors.Crossing;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.SendNotificationsCommand
{
    public class SendNotificationsCommandHandler : IRequestHandler<SendNotificationsCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IHubContext<NotificationHub> _hubcontext;
        private readonly ICurrentUserIdentityService _currentUserIdentity;
        private readonly IAlertConditionDetector<CrossingConditionDetectorParameters> _crossingConditionDetector;

        private TimestampInterval _klineInterval;

        private static readonly IDictionary<int, int> _notificationTriggeredFlags;

        static SendNotificationsCommandHandler()
        {
            _notificationTriggeredFlags = new Dictionary<int, int>();
        }

        public SendNotificationsCommandHandler(
            IBTBDbContext context,
            IEmailService emailService,
            IHubContext<NotificationHub> hubContext,
            ICurrentUserIdentityService currentUserIdentity,
            IAlertConditionDetector<CrossingConditionDetectorParameters> crossingConditionDetector)
        {
            _context = context;
            _emailService = emailService;
            _hubcontext = hubContext;
            _currentUserIdentity = currentUserIdentity;
            _crossingConditionDetector = crossingConditionDetector;
        }

        public async Task<Unit> Handle(SendNotificationsCommand request, CancellationToken cancellationToken)
        {
            _klineInterval = request.KlineInterval;

            foreach (var alert in _context.Alerts)
            {
                if (await AreConditionsMet(alert))
                {
                    if (alert.SendEmail)
                    {
                        await SendEmailMessageAsync(alert);
                    }

                    if (alert.SendInBrowser)
                    {
                        await SendInBrowserNotificationAsync(alert);
                    }
                }
            }

            return Unit.Value;
        }

        private async Task<bool> AreConditionsMet(Alert alert)
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

        private async Task SendInBrowserNotificationAsync(Alert alert)
         =>   await _hubcontext.Clients.User(_currentUserIdentity.UserId)
                    .SendAsync("inbrowser", $"{alert.SymbolPair.PairName} is {alert.Condition} {alert.ValueType} {alert.Value}");

        public static void ResetTriggerFlags()
        {
            foreach (var key in _notificationTriggeredFlags.Keys)
            {
                _notificationTriggeredFlags.Remove(key);
            }
        }
    }
}
