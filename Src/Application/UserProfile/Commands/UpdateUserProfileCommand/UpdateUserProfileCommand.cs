using BTB.Application.UserProfile.Common;
using MediatR;

namespace BTB.Application.UserProfile.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommand : UserProfileInfoRequestBase, IRequest
    {
    }
}