using Application.UnitTests.Common;
using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Application.Common.Interfaces;
using BTB.Persistence;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Application.UnitTests
{
    [Collection("QueryCollection")]
    public class QueryTestsBase : TestsBase
    {
        protected Mock<ICurrentUserIdentityService> GetUserIdentityMock(string userId)
        {
            var userIdentityMock = new Mock<ICurrentUserIdentityService>();
            userIdentityMock
                .Setup(x => x.UserId)
                .Returns(userId);
            return userIdentityMock;
        }
    }
}
