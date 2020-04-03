using BTB.Application.UserProfile.Common;
using MediatR;

namespace BTB.Application.UserProfile.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileCommand : UserProfileInfoRequestBase, IRequest<UserProfileInfoVm>
    {
    }
}