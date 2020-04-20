using Application.UnitTests.Common;
using BTB.Application.Common.Interfaces;
using BTB.Application.ConditionDetectors.Crossing;
using BTB.Application.System.Commands.SendNotificationsCommand;
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
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Price, 2, 1, 3, 0)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Price, 2, 3, 1, 0)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Price, 2, 1, 2, 0)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Price, 2, 2, 3, 0)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Volume, 2, 1, 1, 3)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Volume, 2, 1, 1, 2)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Volume, -2, 1, 1, -3)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Volume, -2, 1, 1, -2)]
        public async Task Handle_ShouldSendNotifications_OnlyWhenCrossingOccurs(TimestampInterval klineInterval,
            AlertValueType alertValueType, decimal threshold, decimal openPrice, decimal closePrice, decimal volume)
        {
            var symbolPairId = 1;
            var userId = "1";

            var kline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = symbolPairId,
                DurationTimestamp = klineInterval,
                OpenPrice = openPrice,
                ClosePrice = closePrice,
                Volume = volume
            };

            var alert = new Alert()
            {
                UserId = userId,
                SymbolPairId = symbolPairId,
                Condition = AlertCondition.Crossing,
                ValueType = alertValueType,
                Value = threshold,
                SendEmail = true,
                Email = "email@email.com",
                Message = "symbol pair 1 crossing 2.0m"
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emailServiceMock = new Mock<IEmailService>();

            var command = new SendNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin };
            var sut = new SendNotificationsCommandHandler(_context, emailServiceMock.Object, _hubcontext, _currentUserIdentity, new CrossingConditionDetector());
            SendNotificationsCommandHandler.ResetTriggerFlags();

            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();

            await AddKline(kline);
            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.Verify(mock => mock.Send(alert.Email, It.IsAny<string>(), alert.Message, _context.EmailTemplates.FirstOrDefault()), Times.Once);

            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Price, 5, 3, 4, 0)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Volume, 5, 1, 1, 4)]
        [InlineData(TimestampInterval.FiveMin, AlertValueType.Volume, -5, 1, 1, -4)]

        public async Task Handle_ShouldNotSendNotifications_WhenCrossingDoesNotOccur(TimestampInterval klineInterval,
            AlertValueType alertValueType, decimal threshold, decimal openPrice, decimal closePrice, decimal volume)
        {
            var symbolPairId = 1;
            var userId = "1";

            var kline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = symbolPairId,
                DurationTimestamp = klineInterval,
                OpenPrice = openPrice,
                ClosePrice = closePrice,
                Volume = volume
            };

            var alert = new Alert()
            {
                UserId = userId,
                SymbolPairId = symbolPairId,
                Condition = AlertCondition.Crossing,
                ValueType = alertValueType,
                Value = threshold,
                SendEmail = true,
                Email = "email@email.com",
                Message = "symbol pair 1 crossing 2.0m"
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emailServiceMock = new Mock<IEmailService>();

            var command = new SendNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin };

            var sut = new SendNotificationsCommandHandler(_context, emailServiceMock.Object, _hubcontext, _currentUserIdentity, new CrossingConditionDetector());

            SendNotificationsCommandHandler.ResetTriggerFlags();

            await sut.Handle(command, CancellationToken.None);
            await AddKline(kline);
            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();
        }

        private Task AddKline(Kline kline)
        {
            _context.Klines.Add(kline);
            return _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
