using Application.UnitTests.Common;
using BTB.Application.Common.Exceptions;
using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.UserProfile.Commands
{
    public class UpdateUserProfileCommandTests : CommandTestsBase
    {
        [Fact]
        public async Task Handle_ShouldUpdateUserProfile_WhenRequestIsValid()
        {
            var userId = "1";
            var expectedUsername = "new_username";
            var expectedProfileBio = "new_bio";
            var expectedFavouriteTradingPair = "BTCUSDT";
            var userAccessorMock = GetUserAccessorMock(userId);

            var sut = new UpdateUserProfileCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);
            var command = new UpdateUserProfileCommand()
            {
                Username = expectedUsername,
                ProfileBio = expectedProfileBio,
                FavouriteTradingPair = expectedFavouriteTradingPair
            };
            await sut.Handle(command, CancellationToken.None);

            var result = _context.UserProfileInfo.SingleOrDefault(i => i.UserId == userId);
            Assert.NotNull(result);
            Assert.Equal(expectedUsername, result.Username);
            Assert.Equal(expectedProfileBio, result.ProfileBio);
            Assert.Equal(expectedFavouriteTradingPair, result.FavouriteTradingPair);
        }


        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenGivenTradingPairDoesNotExist()
        {
            var userId = "1";
            var tradingPair = "AAABBB";
            var userAccessorMock = GetUserAccessorMock(userId);

            var command = new UpdateUserProfileCommand()
            {
                FavouriteTradingPair = tradingPair
            };
            var sut = new UpdateUserProfileCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);

            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserProfileDoesNotExist()
        {
            string userId = null;
            var existingTradingPair = "BTCUSDT";
            var userAccessorMock = GetUserAccessorMock(userId);

            var command = new UpdateUserProfileCommand()
            {
                FavouriteTradingPair = existingTradingPair
            };
            var sut = new UpdateUserProfileCommandHandler(_context, _mapper, _btbBinanceClientMock.Object, userAccessorMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(command, CancellationToken.None));
        }
    }
}
