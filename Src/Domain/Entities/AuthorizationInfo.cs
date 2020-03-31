using System.Collections.Generic;

namespace BTB.Domain.Entities
{
    public class AuthorizationInfo
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }
    }
}