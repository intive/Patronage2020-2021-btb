using Application.UnitTests.Common;
using BTB.Application.Alerts.Commands.DeleteAlertCommand;
using BTB.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Alerts.Commands
{
    public class DeleteAlertCommandTests : CommandTestsBase
    {
        [Fact]
        public async Task Handle_ShouldDeleteAlert()
        {
            var alertId = 2;
            var userId = "1";
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new DeleteAlertCommandHandler(_context, userIdentityMock.Object);
            var command = new DeleteAlertCommand()
            {
                Id = alertId,
            };
            await sut.Handle(command, CancellationToken.None);

            var result = await _context.Alerts.SingleOrDefaultAsync(alert => alert.Id == alertId);
            Assert.Null(result);
            userIdentityMock.VerifyGet(mock => mock.UserId);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAlertDoesNotExist()
        {
            var alertId = -1;
            var userId = "1";
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new DeleteAlertCommandHandler(_context, userIdentityMock.Object);
            var command = new DeleteAlertCommand()
            {
                Id = alertId,
            };

            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(command, CancellationToken.None));
            userIdentityMock.VerifyGet(mock => mock.UserId);
        }
    }
}
