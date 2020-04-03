using Application.UnitTests.Common;
using BTB.Application.Alerts.Commands.CreateAlertCommand;
using BTB.Application.Common.Exceptions;
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
            var expectedCondition = "Condition";
            var expectedValueType = "ValueType";
            var expectedValue = 1.5;
            var expectedSendEmail = true;
            var expectedEmail = "example@mail.com";
            var expectedMessage = "message";
            var expectedUserId = "1";

            var userIdentityMock = GetUserIdentityMock(expectedUserId);

            var sut = new CreateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);
            var command = new CreateAlertCommand()
            {
                SymbolPair = expectedTradingPair,
                Condition = expectedCondition,
                ValueType = expectedValueType,
                Value = expectedValue,
                SendEmail = expectedSendEmail,
                Email = expectedEmail,
                Message = expectedMessage
            };
            var sutResult = await sut.Handle(command, CancellationToken.None);

            var dbResult = _context.Alerts
                .Include(x => x.SymbolPair).ThenInclude(x => x.BuySymbol)
                .Include(x => x.SymbolPair).ThenInclude(x => x.SellSymbol)
                .SingleOrDefault(a => a.UserId == expectedUserId && a.Id == sutResult.Id);

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

            var sut = new CreateAlertCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);
            var command = new CreateAlertCommand()
            {
                SymbolPair = tradingPair
            };

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
            _btbBinanceClientMock.Verify(mock => mock.GetSymbolNames(tradingPair, ""));
        }
    }
}
