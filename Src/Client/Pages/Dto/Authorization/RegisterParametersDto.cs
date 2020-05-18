using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Pages.Dto.Authorization
{
    public class RegisterParametersDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(4, ErrorMessage = "Username needs to be at least 4 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Display Name is required")]
        [MinLength(5, ErrorMessage = "Display Name needs to be at least 5 characters.")]
        [MaxLength(16, ErrorMessage = "Display Name can't be longer than 16 characters.")]
        public string DisplayName { get; set; }

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