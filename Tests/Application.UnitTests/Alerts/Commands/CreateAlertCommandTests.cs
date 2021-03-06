﻿using Application.UnitTests.Common;
using BTB.Application.Alerts.Commands.CreateAlertCommand;
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
    public class CreateAlertCommandTests : CommandTestsBase
    {
        [Fact]
        public async Task Handle_ShouldCreateAlert_WhenRequestIsValid()
        {
            var expectedTradingPair = "BTCUSDT";
            var expectedCondition = AlertCondition.CrossingDown;
            var expectedValueType = AlertValueType.Price;
            var expectedValue = 1.5m;
            var expectedSendEmail = true;
            var expectedEmail = "example@mail.com";
            var expectedUserId = "1";
            var expectedTemplateId = 3;

            var userAccessorMock = GetUserAccessorMock(expectedUserId);

            var sut = new CreateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);
            var command = new CreateAlertCommand()
            {
                SymbolPair = expectedTradingPair,
                Condition = expectedCondition.ToString(),
                ValueType = expectedValueType.ToString(),
                Value = expectedValue,
                SendEmail = expectedSendEmail,
                Email = expectedEmail,
            };
            var sutResult = await sut.Handle(command, CancellationToken.None);

            Alert dbAlert = _context.Alerts
                .Include(x => x.SymbolPair).ThenInclude(x => x.BuySymbol)
                .Include(x => x.SymbolPair).ThenInclude(x => x.SellSymbol)
                .SingleOrDefault(a => a.UserId == expectedUserId && a.Id == sutResult.Id);

            Assert.Equal(expectedTemplateId, dbAlert.MessageTemplateId);

            var dbAlertVo = _mapper.Map<AlertVO>(dbAlert);

            Assert.NotNull(dbAlertVo);
            Assert.Equal(expectedTradingPair, dbAlertVo.SymbolPair);
            Assert.Equal(expectedCondition, dbAlertVo.Condition);
            Assert.Equal(expectedValueType, dbAlertVo.ValueType);
            Assert.Equal(expectedValue, dbAlertVo.Value);
            Assert.Equal(expectedSendEmail, dbAlertVo.SendEmail);
            Assert.Equal(expectedEmail, dbAlertVo.Email);

            userAccessorMock.Verify(x => x.GetCurrentUserId());
            _btbBinanceClientMock.Verify(mock => mock.GetSymbolNames(expectedTradingPair, ""));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenGivenTradingPairDoesNotExist()
        {
            var userId = "user";
            var tradingPair = "AAABBB";
            var userAccessorMock = GetUserAccessorMock(userId);

            var sut = new CreateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);
            var command = new CreateAlertCommand()
            {
                SymbolPair = tradingPair
            };

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
            _btbBinanceClientMock.Verify(mock => mock.GetSymbolNames(tradingPair, ""));
        }
    }
}
