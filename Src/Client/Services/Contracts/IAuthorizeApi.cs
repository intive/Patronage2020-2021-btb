using BTB.Client.Models.Authorization;
using BTB.Client.Pages.Dto;
using System.Threading.Tasks;

namespace BTB.Client.Services.Contracts
{
    public interface IAuthorizeApi
    {
        Task Login(LoginParametersModel loginParameters);

        Task Register(RegisterParametersModel registerParameters);

        Task Logout();

        Task<AuthorizationInfoDto> AuthorizationInfo();
    }
}