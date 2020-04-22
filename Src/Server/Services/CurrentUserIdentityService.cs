using BTB.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BTB.Server.Services
{
    public class CurrentUserIdentityService : ICurrentUserIdentityService
    {
        public ClaimsPrincipal User { get; }
        public string UserId { get; }
        public bool IsAuthenticated { get; }

        public CurrentUserIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            User = httpContextAccessor.HttpContext?.User;
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            IsAuthenticated = UserId != null;
        }
    }
}
