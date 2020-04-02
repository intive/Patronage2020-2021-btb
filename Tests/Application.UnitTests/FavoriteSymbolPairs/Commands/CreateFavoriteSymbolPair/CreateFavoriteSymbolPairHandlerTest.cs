using Application.UnitTests.Common;
using BTB.Application.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Exceptions;

namespace Application.UnitTests.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair
{
    public class CreateFavoriteSymbolPairHandlerTest : CommandTestsBase
    {
        private readonly string _expectedUserId = "userId";
        private readonly int _addedSymbolPairId = 1;
        private readonly int _expectedSymbolPairId = 2;
        private readonly Mock<ICurrentUserIdentityService> _userIdentityMock;
        private readonly CreateFavoriteSymbolPairHandler _sut;

        public CreateFavoriteSymbolPairHandlerTest()
        {
            _userIdentityMock = GetUserIdentityMock(_expectedUserId);
            _sut = new CreateFavoriteSymbolPairHandler(_context, _mapper, _userIdentityMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateFavoriteSymbolPair_WhenRequestIsValid()
        {
            var command = new CreateFavoriteSymbolPairCommand { SymbolPairId = _expectedSymbolPairId };
            await _sut.Handle(command, CancellationToken.None);

            var dbResult = _context.FavoriteSymbolPairs.Find(_expectedUserId, _expectedSymbolPairId);
            Assert.NotNull(dbResult);
            Assert.Equal(_expectedUserId, dbResult.ApplicationUserId);
            Assert.Equal(_expectedSymbolPairId, dbResult.SymbolPairId);

            _userIdentityMock.VerifyGet(x => x.UserId);
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenFavoriteSymbolPairExist()
        {
            var command = new CreateFavoriteSymbolPairCommand { SymbolPairId = _addedSymbolPairId };

            await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Handle(command, CancellationToken.None));
            _userIdentityMock.VerifyGet(x => x.UserId);
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenSymbolPairDontExist()
        {
            var symbolPairIdDontExist = 222;

            var command = new CreateFavoriteSymbolPairCommand { SymbolPairId = symbolPairIdDontExist };

            await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Handle(command, CancellationToken.None));
            _userIdentityMock.VerifyGet(x => x.UserId);
        }
    }
}
