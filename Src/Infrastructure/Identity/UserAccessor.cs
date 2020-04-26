using BTB.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;

namespace BTB.Infrastructure.Identity
{
    public class UserAccessor : IUserAccessor
    {
        private string _userId;
        private string _username;
        private string _userEmail;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _userId = httpContextAccessor.HttpContext?.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            _username = httpContextAccessor.HttpContext?.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            _userEmail = httpContextAccessor.HttpContext?.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }

        public string GetCurrentUserId()
        {
            return _userId;
        }

        public string GetCurrentUserName()
        {
            return _username;
        }

        public string GetCurrentUserEmail()
        {
            return _userEmail;
        }

    }
}
