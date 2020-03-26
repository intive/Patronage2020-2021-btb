namespace BTB.Domain.Entities
{
    public class LoginParameters
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string Token { get; set; }
    }
}
