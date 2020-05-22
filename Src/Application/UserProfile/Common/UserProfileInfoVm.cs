using AutoMapper;
using BTB.Application.Common.Mappings;
using BTB.Domain.Entities;

namespace BTB.Application.UserProfile.Common
{
    public class UserProfileInfoVm : IMapFrom<UserProfileInfo>
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserProfileInfo, UserProfileInfoVm>();
        }
    }
}