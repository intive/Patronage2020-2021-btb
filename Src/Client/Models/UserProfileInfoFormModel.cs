using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Models
{
    public class UserProfileInfoFormModel
    {
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }
    }
}
