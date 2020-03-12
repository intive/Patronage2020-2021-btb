using Application.UnitTests.Common;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
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

            var sut = new CreateUserProfileCommandHandler(_context, _mapper, _binanceClient, userIdentityMock.Object);
            await sut.Handle(new CreateUserProfileCommand()
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
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenGivenTradingPairDoesNotExist()
        {
            var userId = "new_id";
            var tradingPair = "ABCABC";
            var userIdentityMock = GetUserIdentityMock(userId);
            var binanceClientMock = new Mock<IBinanceClient>();
            binanceClientMock.Setup(x => x.GetPriceAsync(tradingPair, CancellationToken.None))
                .Returns(Task.Run(() => WebCallResult<BinancePrice>.CreateErrorResult(new ServerError(404, "pair not found"))));

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

            var sut = new CreateUserProfileCommandHandler(_context, _mapper, _binanceClient, userIdentityMock.Object);

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(new CreateUserProfileCommand(), CancellationToken.None));
        }
    }
}
