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

namespace BTB.Application.System.Commands.SendEmailNotificationsCommand
{
    public class SendNotificationsCommandHandler : IRequestHandler<SendNotificationsCommand>
    {
        private readonly IHubContext<NotificationHub> _hubcontext;
        private readonly ICurrentUserIdentityService _currentUserIdentity;
        private readonly IBTBDbContext _context;
        private readonly IEmailService _emailService;

        private static readonly IDictionary<int, Kline> _pairIdToLastKlineMap;
        private static bool _pairsNotLoaded;
        private static readonly IAlertConditionDetector<CrossingConditionDetectorParameters> _crosssingConditionDetector;

        private TimestampInterval _klineInterval;

        static SendNotificationsCommandHandler()
        {
            _pairsNotLoaded = true;
            _pairIdToLastKlineMap = new Dictionary<int, Kline>();
            _crosssingConditionDetector = new CrossingConditionDetector();
        }

        public SendNotificationsCommandHandler(
            IBTBDbContext context,
            IEmailService emailService, 
            IHubContext<NotificationHub> hubContext,
            ICurrentUserIdentityService currentUserIdentity)
        {
            _context = context;
            _emailService = emailService;
            _hubcontext = hubContext;
            _currentUserIdentity = currentUserIdentity;
        }

        public async Task<Unit> Handle(SendNotificationsCommand request, CancellationToken cancellationToken)
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
                var kline = await GetLastKlineBySymbolPairIdAsync(pair.Id);
                _pairIdToLastKlineMap.Add(pair.Id, kline);
            }
        }

        private async Task SendNotificationsAsync()
        {
            foreach (var alert in _context.Alerts)
            {
                if (alert.SendEmail)
                {
                    if (await AreConditionsMet(alert))
                    {
                        await SendEmailMessageAsync(alert);
                    }
                }

                if (alert.SendInBrowser)
                {
                    if (await AreConditionsMet(alert))
                    {
                        await SendInBrowserNotificationAsync(alert);
                    }
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

        private async Task SendInBrowserNotificationAsync(Alert alert)
        {
            await _hubcontext.Clients.User(_currentUserIdentity.UserId)
                    .SendAsync("inbrowser", 
                        $"{alert.SymbolPair} is {alert.Condition} {alert.ValueType} {alert.Value}");
        }
    }
}
