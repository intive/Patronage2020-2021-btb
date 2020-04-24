using BTB.Client.Pages.Dto.Authorization;
using System.Threading.Tasks;

namespace BTB.Client.Services.Contracts
{
    public interface IAuthorizeService
    {
        Task Login(LoginParametersDto loginParameters);

        Task Register(RegisterParametersDto registerParameters);

        Task Logout();
    }
}