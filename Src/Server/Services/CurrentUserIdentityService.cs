using BTB.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class CurrentUserIdentityService : ICurrentUserIdentityService
    {
        public string UserId { get; }
        public bool IsAuthenticated { get; }

        public CurrentUserIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            IsAuthenticated = UserId != null;
        }
    }
}
