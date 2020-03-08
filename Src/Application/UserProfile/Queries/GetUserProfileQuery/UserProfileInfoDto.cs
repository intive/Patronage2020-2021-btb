using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.UserProfile.Queries.GetUserProfileQuery
{
    public class UserProfileInfoDto
    {
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }
    }
}
