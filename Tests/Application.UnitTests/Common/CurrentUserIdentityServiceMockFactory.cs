using BTB.Application.Common.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests.Common
{
    public abstract class CurrentUserIdentityServiceMockFactory
    {
        public static Mock<ICurrentUserIdentityService> UserIdentityMock
        {
            get
            {
                var _userIdentityMock = new Mock<ICurrentUserIdentityService>();
                MockSetup(ref _userIdentityMock);
                return _userIdentityMock;
            }
        }

        public static void MockSetup(ref Mock<ICurrentUserIdentityService> userIdentityMock)
        {
            userIdentityMock.SetupGet(x => x.UserId)
                .Returns("audit_user");
        }
    }
}
