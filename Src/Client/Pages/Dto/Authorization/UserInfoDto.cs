using System.Collections.Generic;

namespace BTB.Client.Pages.Dto.Authorization
{
    public class UserInfoDto
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }
    }
}
