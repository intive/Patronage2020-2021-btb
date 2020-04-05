using BTB.Application.UserProfile.Common;
using MediatR;

namespace BTB.Application.UserProfile.Queries.GetUserProfileQuery
{
    public class GetUserProfileQuery : IRequest<UserProfileInfoVm>
    {
    }
}