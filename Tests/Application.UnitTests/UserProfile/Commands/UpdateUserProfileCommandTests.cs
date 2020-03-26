using Application.UnitTests.Common;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.UserProfile;
using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using BTB.Domain.ValueObjects;
using CryptoExchange.Net.Objects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static BTB.Application.UserProfile.Commands.UpdateUserProfileCommand.UpdateUserProfileCommand;

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
            var expectedFavouriteTradingPair = "new_pair";
            var userIdentityMock = GetUserIdentityMock(userId);
            var binanceClientMock = new Mock<IBTBBinanceClient>();
            binanceClientMock.Setup(x => x.GetSymbolNames(expectedFavouriteTradingPair, "")).Returns(new SymbolPairVO());

            var sut = new UpdateUserProfileCommandHandler(_context, _mapper, binanceClientMock.Object, userIdentityMock.Object);
            await sut.Handle(new UpdateUserProfileCommand()
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
            var userId = "1";
            var tradingPair = "ABCABC";
            var userIdentityMock = GetUserIdentityMock(userId);
            var binanceClientMock = new Mock<IBTBBinanceClient>();
            binanceClientMock.Setup(x => x.GetSymbolNames(tradingPair, "")).Returns((SymbolPairVO)null);

            var command = new UpdateUserProfileCommand()
            {
                FavouriteTradingPair = tradingPair
            };
            var sut = new UpdateUserProfileCommandHandler(_context, _mapper, binanceClientMock.Object, userIdentityMock.Object);
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserProfileDoesNotExist()
        {
            string userId = null;
            var userIdentityMock = GetUserIdentityMock(userId);
            var binanceClientMock = new Mock<IBTBBinanceClient>();
            binanceClientMock.Setup(x => x.GetSymbolNames(null, "")).Returns(new SymbolPairVO());

            var sut = new UpdateUserProfileCommandHandler(_context, _mapper, binanceClientMock.Object, userIdentityMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(new UpdateUserProfileCommand(), CancellationToken.None));
        }
    }
}
