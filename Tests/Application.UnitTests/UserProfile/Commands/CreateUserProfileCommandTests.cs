using Application.UnitTests.Common;
using BTB.Application.Common.Exceptions;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;
using static BTB.Application.UserProfile.Commands.CreateUserProfileCommand.CreateUserProfileCommand;

namespace Application.UnitTests.UserProfile.Commands
{
    public class CreateUserProfileCommandTests : CommandTestsBase
    {
        private string _userName = "TestUser";
        private string _profileBio = "";
        private string _favouriteTradingPar = "";

        private int userCount()
        {
            return _context.UserProfileInfo.ToList().Count;
        }

        [Fact]
        public async void Average_ProfileShouldBeCreated()
        {
            var handler = new CreateUserProfileCommandHandler(_context, _mapper, _binanceClient);

            int prevUserCount = userCount();
            var result = await handler.Handle(new CreateUserProfileCommand { UserId = 4321, Username = _userName, ProfileBio = _profileBio, FavouriteTradingPair = _favouriteTradingPar }, CancellationToken.None);
            int nextUserCount = userCount();

            Assert.True(prevUserCount.Equals(nextUserCount - 1));
        }

        [Fact]
        public async void Bad_UserExists_ShouldNotCreateUserAndThrowBadRequest()
        {
            var handler = new CreateUserProfileCommandHandler(_context, _mapper, _binanceClient);
            bool exceptionThrown = false;

            int prevUserCount = userCount();
            try
            {
                var result = await handler.Handle(new CreateUserProfileCommand { UserId = 2, Username = _userName, ProfileBio = _profileBio, FavouriteTradingPair = _favouriteTradingPar }, CancellationToken.None);
            }
            catch (BadRequestException e)
            {
                exceptionThrown = true;
            }
            int nextUserCount = userCount();

            Assert.True(exceptionThrown);
            Assert.True(prevUserCount.Equals(nextUserCount));
        }
    }
}
