﻿using BTB.Application.Common.Interfaces;
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

                if (await AreConditionsMet(alert))
                {
                    await SendEmailMessageAsync(alert);
                }
            }

            return Unit.Value;
        }
        private async Task<bool> AreConditionsMet(Alert alert)
        {
            IList<Kline> lastKlines = await GetLastTwoKlinesBySymbolPairIdAsync(alert.SymbolPairId);
            if (lastKlines.Count != 2)
            {
                return false;
            }

            var crossingParameters = new CrossingConditionDetectorParameters()
            {
                NewKline = lastKlines[0],
                OldKline = lastKlines[1]
            };

            if (_crossingConditionDetector.IsConditionMet(alert, crossingParameters))
            {
                return true;
            }

            return false;
        }

        private Task<List<Kline>> GetLastTwoKlinesBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .OrderByDescending(kline => kline.OpenTimestamp)
                .Where(kline => kline.SymbolPairId == symbolPairId && kline.DurationTimestamp == _klineInterval)
                .Take(2)
                .ToListAsync();
        }

        private async Task SendEmailMessageAsync(Alert alert)
        {
            EmailTemplate template = await _context.EmailTemplates.SingleOrDefaultAsync();
            _emailService.Send(alert.Email, "BTB trading pair alert", alert.Message, template);
        }
    }
}
