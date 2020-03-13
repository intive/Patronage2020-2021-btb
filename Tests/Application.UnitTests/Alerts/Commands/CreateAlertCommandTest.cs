using Application.UnitTests.Common;
using BTB.Application.Alerts.Commands.CreateAlert;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static BTB.Application.Alerts.Commands.CreateAlert.CreateAlertCommand;

namespace Application.UnitTests.Alerts.Commands
{
    public class CreateAlertCommandTest : CommandTestsBase
    {
        [Fact]
        public async Task Handle_ShouldCreateAlert_WhenRequestIsValid()
        {
            var expectedSymbol = "Symbol";
            var expectedCondition = "Condition";
            var expectedValueType = "ValueType";
            var expectedValue = 1.5;
            var expectedSendEmail = true;
            var expectedEmail = "example@gmail.com";
            var expectedMessage = "message";
            var expectedUserId = "xxx";

            var userIdentityMock = GetUserIdentityMock(expectedUserId);

            var sut = new CreateAlertCommandHandler(_context, _mapper, userIdentityMock.Object);
            await sut.Handle(new CreateAlertCommand()
            {
                Symbol = expectedSymbol,
                Condition = expectedCondition,
                ValueType = expectedValueType,
                Value = expectedValue,
                SendEmail = expectedSendEmail,
                Email = expectedEmail,
                Message = expectedMessage

            }, CancellationToken.None);

            var result = _context.Alerts.SingleOrDefault(a => a.UserId == expectedUserId);

            Assert.NotNull(result);
            userIdentityMock.VerifyGet(x => x.UserId);
            Assert.Equal(expectedSymbol, result.Symbol);
            Assert.Equal(expectedCondition, result.Condition);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedSendEmail, result.SendEmail);
            Assert.Equal(expectedEmail, result.Email);
            Assert.Equal(expectedMessage, result.Message);
        }
    }
}
