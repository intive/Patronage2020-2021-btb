using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Pages.Dto.Authorization
{
    public class RegisterParametersDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(4, ErrorMessage = "Username has to be at least 4 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Must be at least 4 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [MinLength(4, ErrorMessage = "Must be at least 4 characters")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords should be the same.")]
        public string PasswordConfirm { get; set; }
    }
}