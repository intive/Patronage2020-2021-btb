using Application.UnitTests.Common;
using BTB.Application.Common.Exceptions;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
            var expectedFavouriteTradingPair = "BTCUSDT";
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new CreateUserProfileCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);
            var command = new CreateUserProfileCommand()
            {
                Username = expectedUsername,
                ProfileBio = expectedProfileBio,
                FavouriteTradingPair = expectedFavouriteTradingPair
            };
            var sutResult = await sut.Handle(command, CancellationToken.None);

            var dbResult = _context.UserProfileInfo.SingleOrDefault(i => i.UserId == userId);
            Assert.NotNull(dbResult);
            Assert.Equal(expectedUsername, dbResult.Username);
            Assert.Equal(expectedProfileBio, dbResult.ProfileBio);
            Assert.Equal(expectedFavouriteTradingPair, dbResult.FavouriteTradingPair);

            Assert.Equal(userId, sutResult.UserId);
            Assert.Equal(expectedUsername, sutResult.Username);
            Assert.Equal(expectedProfileBio, sutResult.ProfileBio);
            Assert.Equal(expectedFavouriteTradingPair, sutResult.FavouriteTradingPair);
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenGivenTradingPairDoesNotExist()
        {
            var userId = "new_id";
            var tradingPair = "AAABBB";
            var userIdentityMock = GetUserIdentityMock(userId);

            var command = new CreateUserProfileCommand()
            {
                FavouriteTradingPair = tradingPair
            };
            var sut = new CreateUserProfileCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenUserProfileAlreadyExists()
        {
            var existingUserId = "1";
            var existingTrdingPair = "BTCUSDT";
            var userIdentityMock = GetUserIdentityMock(existingUserId);

            var command = new CreateUserProfileCommand()
            {
                FavouriteTradingPair = existingTrdingPair
            };
            var sut = new CreateUserProfileCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userIdentityMock.Object);

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
        }
    }
}
