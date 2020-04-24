using System.Threading.Tasks;
using Xunit;
using BTB.Application.UserProfile.Queries.GetUserProfileQuery;
using System.Threading;
using BTB.Application.Common.Exceptions;

namespace Application.UnitTests.UserProfile.Queries
{
    public class GetUserProfileQueryTests : QueryTestsBase
    {
        [Fact]
        public async Task Handle_ShouldReturnUserProfileInfo_WhenUserProfileExists()
        {
            var userId = "1";
            var expectedUsername = "UserOne";
            var expectedProfileBio = "";
            var expectedFavouriteTradingPair = "";
            var userAccessorMock = GetUserAccessorMock(userId);

            var sut = new GetUserProfileQueryHandler(_context, _mapper, userAccessorMock.Object);
            var result = await sut.Handle(new GetUserProfileQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedUsername, result.Username);
            Assert.Equal(expectedProfileBio, result.ProfileBio);
            Assert.Equal(expectedFavouriteTradingPair, result.FavouriteTradingPair);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserProfileDoesNotExist()
        {
            var userAccessorMock = GetUserAccessorMock(null);

            var sut = new GetUserProfileQueryHandler(_context, _mapper, userAccessorMock.Object);
            
            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(new GetUserProfileQuery(), CancellationToken.None));
        }
    }
}
