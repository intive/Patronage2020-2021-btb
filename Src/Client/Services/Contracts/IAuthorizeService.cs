using BTB.Client.Models.Authorization;
using System.Threading.Tasks;

namespace BTB.Client.Services.Contracts
{
    public interface IAuthorizeService
    {
        Task Login(LoginParametersModel loginParameters);

        Task Register(RegisterParametersModel registerParameters);

        Task Logout();
    }
}