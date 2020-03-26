using Application.UnitTests.Common;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
using BTB.Domain.ValueObjects;
using CryptoExchange.Net.Objects;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static BTB.Application.UserProfile.Commands.CreateUserProfileCommand.CreateUserProfileCommand;

namespace Application.UnitTests.UserProfile.Commands
{
    public class CreateUserProfileCommandTests : CommandTestsBase
    {
        [Fact]
        public async Task Handle_ShouldCreateUserProfile_WhenRequestIsValid()
        {
            var userId = "new_id";
            var expectedUsername = userId + "_username";
            var expectedProfileBio = userId + "_bio";
            var expectedFavouriteTradingPair = userId + "_pair";
            var userIdentityMock = GetUserIdentityMock(userId);

            var binanceClientMock = new Mock<IBTBBinanceClient>();
            binanceClientMock.Setup(x => x.GetSymbolNames(expectedFavouriteTradingPair, "")).Returns(new SymbolPairVO());

            var sut = new CreateUserProfileCommandHandler(_context, _mapper, binanceClientMock.Object, userIdentityMock.Object);
            var sutResult = await sut.Handle(new CreateUserProfileCommand()
            {
                Username = expectedUsername,
                ProfileBio = expectedProfileBio,
                FavouriteTradingPair = expectedFavouriteTradingPair
            }, CancellationToken.None);

            var result = _context.UserProfileInfo.SingleOrDefault(i => i.UserId == userId);
            Assert.NotNull(result);
            Assert.Equal(expectedUsername, result.Username);
            Assert.Equal(expectedProfileBio, result.ProfileBio);
            Assert.Equal(expectedFavouriteTradingPair, result.FavouriteTradingPair);

            Assert.Equal(userId, sutResult.UserId);
            Assert.Equal(expectedUsername, sutResult.Username);
            Assert.Equal(expectedProfileBio, sutResult.ProfileBio);
            Assert.Equal(expectedFavouriteTradingPair, sutResult.FavouriteTradingPair);
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenGivenTradingPairDoesNotExist()
        {
            var userId = "new_id";
            var tradingPair = "ABCABC";
            var userIdentityMock = GetUserIdentityMock(userId);
            var binanceClientMock = new Mock<IBTBBinanceClient>();
            binanceClientMock.Setup(x => x.GetSymbolNames(tradingPair, "")).Returns((SymbolPairVO)null);

            var command = new CreateUserProfileCommand()
            {
                FavouriteTradingPair = tradingPair
            };
            var sut = new CreateUserProfileCommandHandler(_context, _mapper, binanceClientMock.Object, userIdentityMock.Object);
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenUserProfileAlreadyExists()
        {
            var existingUserId = "1";
            var userIdentityMock = GetUserIdentityMock(existingUserId);
            var binanceClientMock = new Mock<IBTBBinanceClient>();
            binanceClientMock.Setup(x => x.GetSymbolNames(null, "")).Returns(new SymbolPairVO());

            var sut = new CreateUserProfileCommandHandler(_context, _mapper, binanceClientMock.Object, userIdentityMock.Object);

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(new CreateUserProfileCommand(), CancellationToken.None));
        }
    }
}
