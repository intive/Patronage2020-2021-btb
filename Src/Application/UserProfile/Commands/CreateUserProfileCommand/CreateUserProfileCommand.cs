using BTB.Application.UserProfile.Common;
using MediatR;

namespace BTB.Application.UserProfile.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileCommand : IRequest<UserProfileInfoVm>
    {
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }
    }
}