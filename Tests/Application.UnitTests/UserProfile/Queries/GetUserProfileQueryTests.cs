﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BTB.Application.UserProfile.Queries.GetUserProfileQuery;
using static BTB.Application.UserProfile.Queries.GetUserProfileQuery.GetUserProfileQuery;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading;
using BTB.Application.Common.Exceptions;
using BTB.Application.UserProfile;
using BTB.Application.Common.Interfaces;

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
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new GetUserProfileQueryHandler(_context, _mapper, userIdentityMock.Object);
            var result = await sut.Handle(new GetUserProfileQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedUsername, result.Username);
            Assert.Equal(expectedProfileBio, result.ProfileBio);
            Assert.Equal(expectedFavouriteTradingPair, result.FavouriteTradingPair);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserProfileDoesNotExist()
        {
            var userIdentityMock = GetUserIdentityMock(null);

            var sut = new GetUserProfileQueryHandler(_context, _mapper, userIdentityMock.Object);
            
            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(new GetUserProfileQuery(), CancellationToken.None));
        }
    }
}