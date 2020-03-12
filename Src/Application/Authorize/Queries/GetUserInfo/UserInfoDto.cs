using System.Collections.Generic;

namespace BTB.Application.Authorize.Queries.GetUserInfo
{
    public class UserInfoVm
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }
    }
}