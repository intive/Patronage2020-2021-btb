namespace BTB.Application.Authorize.Commands.Login
{
    public class LoginParametersDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}