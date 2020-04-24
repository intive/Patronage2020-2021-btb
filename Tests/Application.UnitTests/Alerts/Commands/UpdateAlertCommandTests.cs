﻿using Application.UnitTests.Common;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.Common.Exceptions;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using BTB.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Alerts.Commands
{
    public class UpdateAlertCommandTests : CommandTestsBase
    {
        [Fact]
        public async Task Handle_ShouldUpdateAlert_WhenRequestIsValid()
        {
            var alertId = 1;
            var expectedUserId = "1";
            var expectedTradingPair = "BTCUSDT";
            var expectedCondition = "Crossing";
            var expectedValueType = "Price";
            var expectedValue = 1234.56m;
            var expectedSendEmail = true;
            var expectedEmail = "newexample@newmail.com";
            var expectedMessage = "new_message";

            var userAccessorMock = GetUserAccessorMock(expectedUserId);

            var sut = new UpdateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);
            var command = new UpdateAlertCommand()
            {
                Id = alertId,
                SymbolPair = expectedTradingPair,
                Condition = expectedCondition,
                ValueType = expectedValueType,
                Value = expectedValue,
                SendEmail = expectedSendEmail,
                Email = expectedEmail,
                Message = expectedMessage
            };

            await sut.Handle(command, CancellationToken.None);
            Alert dbAlert = _context.Alerts
                .Include(x => x.SymbolPair).ThenInclude(x => x.BuySymbol)
                .Include(x => x.SymbolPair).ThenInclude(x => x.SellSymbol)
                .SingleOrDefault(a => a.UserId == expectedUserId && a.Id == alertId);

            var dbAlertVo = _mapper.Map<AlertVO>(dbAlert);

            Assert.NotNull(dbAlertVo);
            Assert.Equal(expectedTradingPair, dbAlertVo.SymbolPair);
            Assert.Equal(expectedCondition, dbAlertVo.Condition);
            Assert.Equal(expectedValueType, dbAlertVo.ValueType);
            Assert.Equal(expectedValue, dbAlertVo.Value);
            Assert.Equal(expectedSendEmail, dbAlertVo.SendEmail);
            Assert.Equal(expectedEmail, dbAlertVo.Email);
            Assert.Equal(expectedMessage, dbAlertVo.Message);

            userAccessorMock.Verify(x => x.GetCurrentUserId());
            _btbBinanceClientMock.Verify(mock => mock.GetSymbolNames(expectedTradingPair, ""));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenGivenTradingPairDoesNotExist()
        {
            var userId = "user";
            var tradingPair = "AAABBB";
            var userAccessorMock = GetUserAccessorMock(userId);

            var sut = new UpdateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);
            var command = new UpdateAlertCommand()
            {
                SymbolPair = tradingPair
            };

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
            _btbBinanceClientMock.Verify(mock => mock.GetSymbolNames(tradingPair, ""));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAlertDoesNotExist()
        {
            var userId = "";
            var existingTradingPair = "BTCUSDT";
            var userAccessorMock = GetUserAccessorMock(userId);

            var sut = new UpdateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);
            var command = new UpdateAlertCommand()
            {
                SymbolPair = existingTradingPair
            };

            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(command, CancellationToken.None));
            userAccessorMock.Verify(mock => mock.GetCurrentUserId());
        }
    }
}
