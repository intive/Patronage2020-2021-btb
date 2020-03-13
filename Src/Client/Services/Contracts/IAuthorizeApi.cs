using BTB.Client.Pages.Dto.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Services.Contracts
{
    public interface IAuthorizeApi
    {
        Task Login(LoginParametersDto loginParameters);

        Task Register(RegisterParametersDto registerParameters);

        Task Logout();

        Task<UserInfoDto> GetUserInfo();
    }
}