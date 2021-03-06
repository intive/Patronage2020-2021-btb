﻿using Application.UnitTests.Common;
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
        [Fact(Skip = "demo preparations")]
        public async Task Handle_ShouldSendNotifications_WhenConditionsAreMet()
        {
            var symbolPairId = 1;
            var userId = "1";

            var lastKline = new Kline()
            {
                OpenTimestamp = 2,
                SymbolPairId = symbolPairId,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = 1,
                ClosePrice = 3,
                Volume = 1
            };

            var secondLastKline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = symbolPairId,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = 1,
                ClosePrice = 3,
                Volume = 1
            };

            var alert = new Alert()
            {
                UserId = userId,
                SymbolPairId = symbolPairId,
                Condition = AlertCondition.Crossing,
                ValueType = AlertValueType.Price,
                Value = 2,
                SendEmail = true,
                Email = "email@email.com",
                TriggerOnce = false,
                WasTriggered = false
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emailServiceMock = new Mock<IEmailService>();
            var emailKeeperMock = new Mock<IEmailKeeper>();

            var command = new SendEmailNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin };
            var sut = new SendEmailNotificationsCommandHandler(_context, emailServiceMock.Object, emailKeeperMock.Object);
            SendEmailNotificationsCommandHandler.ResetTriggerFlags();

            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();

            await AddKline(lastKline);
            await AddKline(secondLastKline);
            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.Verify(mock => mock.Send(alert.Email, It.IsAny<string>(), It.IsAny<string>(), _context.EmailTemplates.FirstOrDefault()), Times.Once);

            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "demo preparations")]
        public async Task Handle_ShouldNotSendNotifications_WhenConditionsAreNotMet()
        {
            var symbolPairId = 1;
            var userId = "1";

            var lastKline = new Kline()
            {
                OpenTimestamp = 2,
                SymbolPairId = symbolPairId,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = 1,
                ClosePrice = 2,
                Volume = 1
            };

            var secondLastKline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = symbolPairId,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = 1,
                ClosePrice = 2,
                Volume = 1
            };

            var alert = new Alert()
            {
                UserId = userId,
                SymbolPairId = symbolPairId,
                Condition = AlertCondition.Crossing,
                ValueType = AlertValueType.Price,
                Value = 3,
                SendEmail = true,
                Email = "email@email.com",
                TriggerOnce = false,
                WasTriggered = false
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emailServiceMock = new Mock<IEmailService>();
            var emailKeeperMock = new Mock<IEmailKeeper>();

            var command = new SendEmailNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin };
            var sut = new SendEmailNotificationsCommandHandler(_context, emailServiceMock.Object, emailKeeperMock.Object);
            SendEmailNotificationsCommandHandler.ResetTriggerFlags();

            await sut.Handle(command, CancellationToken.None);
            await AddKline(lastKline);
            await AddKline(secondLastKline);
            await sut.Handle(command, CancellationToken.None);
            emailServiceMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "demo preparations")]
        public async Task Handle_ShouldNotSendNotifications_WhenSendEmailPropertyIsSetToNull()
        {
            var symbolPairId = 1;
            var userId = "1";

            var lastKline = new Kline()
            {
                OpenTimestamp = 2,
                SymbolPairId = symbolPairId,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = 1,
                ClosePrice = 3,
                Volume = 1
            };

            var secondlastKline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = symbolPairId,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = 1,
                ClosePrice = 3,
                Volume = 1
            };

            var alert = new Alert()
            {
                UserId = userId,
                SymbolPairId = symbolPairId,
                Condition = AlertCondition.Crossing,
                ValueType = AlertValueType.Price,
                Value = 2,
                SendEmail = false,
                Email = null,
                TriggerOnce = false,
                WasTriggered = false
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emailServiceMock = new Mock<IEmailService>();
            var emailKeeperMock = new Mock<IEmailKeeper>();

            var command = new SendEmailNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin };
            var sut = new SendEmailNotificationsCommandHandler(_context, emailServiceMock.Object, emailKeeperMock.Object);
            SendEmailNotificationsCommandHandler.ResetTriggerFlags();

            await sut.Handle(command, CancellationToken.None);
            await AddKline(lastKline);
            await AddKline(secondlastKline);
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
