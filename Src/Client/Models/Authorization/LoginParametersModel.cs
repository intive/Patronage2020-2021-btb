﻿namespace BTB.Client.Models.Authorization
{
    public class LoginParametersModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}