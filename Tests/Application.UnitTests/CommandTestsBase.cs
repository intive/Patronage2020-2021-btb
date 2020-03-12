using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Mappings;
using BTB.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests.Common
{
    public class CommandTestsBase : TestsBase
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
