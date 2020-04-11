using Application.UnitTests.Common;
using BTB.Application.Common.Interfaces;
using BTB.Application.ConditionDetectors.Crossing;
using BTB.Application.System.Commands.SendEmailNotificationsCommand;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.System.Commands
{
    public class SendEmailNotificationsCommandTests : CommandTestsBase
    {
        [Theory]
        [InlineData(AlertValueType.Price, 1, 1, 3, 1, 1, 1)]
        [InlineData(AlertValueType.Volume, 1, 1, 1, 3, 1, 1)]
        public async Task Handle_ShouldSendNotifications_WhenCrossingOccurs(
            AlertValueType alertValueType,
            decimal kline1closePrice, decimal kline1volume,
            decimal kline2closePrice, decimal kline2volume,
            decimal kline3closePrice, decimal kline3volume)
        {
            var symbolPairId = 1;
            var userId = "1";

            var values = new List<(decimal closePrice, decimal volume)>()
            {
                (kline1closePrice, kline1volume),
                (kline2closePrice, kline2volume),
                (kline3closePrice, kline3volume),
            };
            IList<Kline> mockKlines = CreateMockKlines(values, symbolPairId, TimestampInterval.FiveMin);

            var alert = new Alert()
            {
                UserId = userId,
                SymbolPairId = symbolPairId,
                Condition = AlertCondition.Crossing,
                ValueType = alertValueType,
                Value = 2.0m,
                SendEmail = true,
                Email = "email@email.com",
                Message = "symbol pair 1 crossing 2.0m"
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emailServiceMock = new Mock<IEmailService>();

            var command = new SendEmailNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin };
            var sut = new SendEmailNotificationsCommandHandler(_context, emailServiceMock.Object,
                new CrossingConditionDetector());
            SendEmailNotificationsCommandHandler.ResetTriggerFlags();

            await sut.Handle(command, CancellationToken.None);
            await AddKline(mockKlines[0]);
            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();

            await AddKline(mockKlines[1]);
            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.Verify(mock => mock.Send(alert.Email, It.IsAny<string>(), alert.Message, _context.EmailTemplates.FirstOrDefault()), Times.Once);

            await AddKline(mockKlines[2]);
            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.Verify(mock => mock.Send(alert.Email, It.IsAny<string>(), alert.Message, _context.EmailTemplates.FirstOrDefault()), Times.Exactly(2));

            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(AlertValueType.Price, 1, 1, 1, 1, 1, 1)]
        [InlineData(AlertValueType.Volume, 1, 1, 1, 1, 1, 1)]
        [InlineData(AlertValueType.Price, 3, 3, 3, 3, 3, 3)]
        [InlineData(AlertValueType.Volume, 3, 3, 3, 3, 3, 3)]
        [InlineData(AlertValueType.Price, 2, 1, 2, 1, 3, 1)]
        
        public async Task Handle_ShouldNotSendNotifications_WhenCrossingDoesNotOccur(
            AlertValueType alertValueType,
            decimal kline1closePrice, decimal kline1volume,
            decimal kline2closePrice, decimal kline2volume,
            decimal kline3closePrice, decimal kline3volume)
        {
            var symbolPairId = 1;
            var userId = "1";

            var values = new List<(decimal closePrice, decimal volume)>()
            {
                (kline1closePrice, kline1volume),
                (kline2closePrice, kline2volume),
                (kline3closePrice, kline3volume),
            };
            IList<Kline> mockKlines = CreateMockKlines(values, symbolPairId, TimestampInterval.FiveMin);

            var alert = new Alert()
            {
                UserId = userId,
                SymbolPairId = symbolPairId,
                Condition = AlertCondition.Crossing,
                ValueType = alertValueType,
                Value = 2.0m,
                SendEmail = true,
                Email = "email@email.com",
                Message = "symbol pair 1 crossing 2.0m - volume"
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emailServiceMock = new Mock<IEmailService>();

            var command = new SendEmailNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin };
            var sut = new SendEmailNotificationsCommandHandler(_context, emailServiceMock.Object,
                new CrossingConditionDetector());

            await AddKline(mockKlines[0]);
            await sut.Handle(command, CancellationToken.None);
            await AddKline(mockKlines[1]);
            await sut.Handle(command, CancellationToken.None);
            await AddKline(mockKlines[2]);
            await sut.Handle(command, CancellationToken.None);

            emailServiceMock.VerifyNoOtherCalls();
        }

        private IList<Kline> CreateMockKlines(IList<(decimal closePrice, decimal volume)> klinesValues, int symbolPairId, TimestampInterval klineInterval)
        {
            var klines = new List<Kline>();

            var index = 1;
            foreach (var values in klinesValues)
            {
                klines.Add(new Kline()
                {
                    OpenTimestamp = index++,
                    SymbolPairId = symbolPairId,
                    DurationTimestamp = klineInterval,
                    ClosePrice = values.closePrice,
                    Volume = values.volume
                });
            }

            return klines;
        }

        private Task AddKline(Kline kline)
        {
            _context.Klines.Add(kline);
            return _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
