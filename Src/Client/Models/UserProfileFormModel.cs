using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Models
{
    public class UserProfileFormModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 16 characters long.")]
        public string Username { get; set; }
        [StringLength(256, ErrorMessage = "Profile bio cannot be longer than 256 characters.")]
        public string ProfileBio { get; set; }
        [StringLength(10, ErrorMessage = "Trading pair symbol cannot be longer than 10 characters.")]
        [RegularExpression("^$|^([A-Z]{5,10})$", ErrorMessage = "Trading pair format is incorrect.")]
        public string FavouriteTradingPair { get; set; }
    }
}
