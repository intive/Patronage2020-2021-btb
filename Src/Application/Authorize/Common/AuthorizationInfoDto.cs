﻿using System.Collections.Generic;

namespace BTB.Application.Authorize.Common
{
    public class AuthorizationInfoDto
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }
    }
}