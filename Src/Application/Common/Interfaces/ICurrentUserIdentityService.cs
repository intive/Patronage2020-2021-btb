using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Interfaces
{
    public interface ICurrentUserIdentityService
    {
        string UserId { get; }
        bool IsAuthenticated { get; }
    }
}
