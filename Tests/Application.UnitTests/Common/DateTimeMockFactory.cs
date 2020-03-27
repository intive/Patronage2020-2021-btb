using BTB.Common;
using Moq;
using System;

namespace Application.UnitTests.Common
{
    public abstract class DateTimeMockFactory
    {
        public static Mock<IDateTime> DateTimeMock
        {
            get
            {
                var _dateTimeMock = new Mock<IDateTime>();
                MockSetup(ref _dateTimeMock);
                return _dateTimeMock;
            }
        }

        public static void MockSetup(ref Mock<IDateTime> dateTimeMock)
        {
            dateTimeMock.SetupGet(x => x.Now)
                .Returns(DateTime.UtcNow);
        }
    }
}
