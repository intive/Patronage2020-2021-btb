using MediatR;

namespace BTB.Application.UserProfile.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommand : IRequest
    {
        public string Username { get; set; }
        public string ProfileBio { get; set; }
        public string FavouriteTradingPair { get; set; }
    }
}