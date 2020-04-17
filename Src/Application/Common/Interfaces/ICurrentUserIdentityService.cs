using System.Security.Claims;

namespace BTB.Application.Common.Interfaces
{
    public interface ICurrentUserIdentityService
    {
        ClaimsPrincipal User { get; }
        string UserId { get; }
        bool IsAuthenticated { get; }
    }
}
