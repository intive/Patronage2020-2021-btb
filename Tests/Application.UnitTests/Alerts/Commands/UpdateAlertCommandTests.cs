using Application.UnitTests.Common;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static BTB.Application.Alerts.Commands.UpdateAlertCommand.UpdateAlertCommand;

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
            var expectedCondition = "new_condition";
            var expectedValueType = "new_value_type";
            var expectedValue = 1234.56;
            var expectedSendEmail = true;
            var expectedEmail = "newexample@newmail.com";
            var expectedMessage = "new_message";

            var userIdentityMock = GetUserIdentityMock(expectedUserId);

            var sut = new UpdateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);
            var command = new UpdateAlertCommand()
            {
                Id = alertId,
                Symbol = expectedTradingPair,
                Condition = expectedCondition,
                ValueType = expectedValueType,
                Value = expectedValue,
                SendEmail = expectedSendEmail,
                Email = expectedEmail,
                Message = expectedMessage
            };

            await sut.Handle(command, CancellationToken.None);
            var dbResult = _context.Alerts.SingleOrDefault(a => a.UserId == expectedUserId && a.Id == alertId);
            Assert.NotNull(dbResult);
            Assert.Equal(expectedTradingPair, dbResult.Symbol);
            Assert.Equal(expectedCondition, dbResult.Condition);
            Assert.Equal(expectedValueType, dbResult.ValueType);
            Assert.Equal(expectedValue, dbResult.Value);
            Assert.Equal(expectedSendEmail, dbResult.SendEmail);
            Assert.Equal(expectedEmail, dbResult.Email);
            Assert.Equal(expectedMessage, dbResult.Message);

            userIdentityMock.VerifyGet(x => x.UserId);
            _btbBinanceClientMock.Verify(mock => mock.GetSymbolNames(expectedTradingPair, ""));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenGivenTradingPairDoesNotExist()
        {
            var userId = "user";
            var tradingPair = "AAABBB";
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new UpdateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);
            var command = new UpdateAlertCommand()
            {
                Symbol = tradingPair
            };

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
            _btbBinanceClientMock.Verify(mock => mock.GetSymbolNames(tradingPair, ""));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAlertDoesNotExist()
        {
            var userId = "";
            var existingTradingPair = "BTCUSDT";
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new UpdateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);
            var command = new UpdateAlertCommand()
            {
                Symbol = existingTradingPair
            };

            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(command, CancellationToken.None));
            userIdentityMock.VerifyGet(mock => mock.UserId);
        }
    }
}
