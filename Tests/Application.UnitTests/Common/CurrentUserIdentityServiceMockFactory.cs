using BTB.Application.Common.Interfaces;
using Moq;

namespace Application.UnitTests.Common
{
    public abstract class UserAccessorMockFactory
    {
        public static Mock<IUserAccessor> UserAccessorMock
        {
            get
            {
                var _userAccessorMock = new Mock<IUserAccessor>();
                MockSetup(ref _userAccessorMock);
                return _userAccessorMock;
            }
        }

        public static void MockSetup(ref Mock<IUserAccessor> userAccessorMock)
        {
            userAccessorMock.Setup(x => x.GetCurrentUserId())
                .Returns("audit_user");
        }
    }
}
