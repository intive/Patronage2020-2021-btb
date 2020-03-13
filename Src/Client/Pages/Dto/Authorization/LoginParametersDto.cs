using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Pages.Dto.Authorization
{
    public class LoginParametersDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
