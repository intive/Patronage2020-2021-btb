using Application.UnitTests.Common;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.Common.Exceptions;
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
                SymbolPair = expectedTradingPair,
                Condition = expectedCondition,
                ValueType = expectedValueType,
                Value = expectedValue,
                SendEmail = expectedSendEmail,
                Email = expectedEmail,
                Message = expectedMessage
            };

            await sut.Handle(command, CancellationToken.None);
            var dbResult = _context.Alerts
                .Include(x => x.SymbolPair).ThenInclude(x => x.BuySymbol)
                .Include(x => x.SymbolPair).ThenInclude(x => x.SellSymbol)
                .SingleOrDefault(a => a.UserId == expectedUserId && a.Id == alertId);

            Assert.NotNull(dbResult);
            Assert.Equal(expectedTradingPair, dbResult.SymbolPair.PairName);
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
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new UpdateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);
            var command = new UpdateAlertCommand()
            {
                SymbolPair = existingTradingPair
            };

            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(command, CancellationToken.None));
            userIdentityMock.VerifyGet(mock => mock.UserId);
        }
    }
}
