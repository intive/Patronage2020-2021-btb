
namespace BTB.Application.Common.Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUserId();
        string GetCurrentUserName();
        string GetCurrentUserEmail();
    }
}
