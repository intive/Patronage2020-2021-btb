using Application.UnitTests.Common;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair;
using BTB.Application.FavoriteSymbolPairs.Commands.DeleteFavoriteSymbolPair;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.FavoriteSymbolPairs.Commands.DeleteFavoriteSymbolPair
{
    public class DeleteFavoriteSymbolPairHandlerTest : CommandTestsBase
    {
        private readonly string _expectedUserId = "userId";
        private readonly int _addedSymbolPairId = 1;
        private readonly Mock<ICurrentUserIdentityService> _userIdentityMock;
        private readonly DeleteFavoriteSymbolPairHandler _sut;

        public DeleteFavoriteSymbolPairHandlerTest()
        {
            _userIdentityMock = GetUserIdentityMock(_expectedUserId);
            _sut = new DeleteFavoriteSymbolPairHandler(_context, _userIdentityMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteFavoriteSymbolPair()
        {
            var command = new DeleteFavoriteSymbolPairCommand() { SymbolPairId = _addedSymbolPairId };
            await _sut.Handle(command, CancellationToken.None);

            var result = _context.FavoriteSymbolPairs.Find(_expectedUserId, _addedSymbolPairId);

            Assert.Null(result);
            _userIdentityMock.VerifyGet(x => x.UserId);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenFavoriteSymbolPairDoesNotExist()
        {
            var notAddedSymbolPairId = 2;
            var command = new DeleteFavoriteSymbolPairCommand() { SymbolPairId = notAddedSymbolPairId };

            await Assert.ThrowsAsync<NotFoundException>(async () => await _sut.Handle(command, CancellationToken.None));
            _userIdentityMock.VerifyGet(x => x.UserId);
        }
    }
}
