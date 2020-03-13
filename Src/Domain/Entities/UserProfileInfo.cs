using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Entities
{
    public class UserProfileInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }
    }
}
