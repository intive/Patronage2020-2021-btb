using BTB.Client.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BTB.Client.Pages.Dto.Authorization;

namespace BTB.Client.States
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private AuthorizationInfoDto _authInfoCache;
        private readonly IAuthorizeApi _authorizeApi;

        public IdentityAuthenticationStateProvider(IAuthorizeApi authorizeApi)
        {
            this._authorizeApi = authorizeApi;
        }

        public async Task Login(LoginParametersDto loginParameters)
        {
            await _authorizeApi.Login(loginParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterParametersDto registerParameters)
        {
            await _authorizeApi.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            await _authorizeApi.Logout();
            _authInfoCache = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<AuthorizationInfoDto> GetAuthorizationInfo()
        {
            if (_authInfoCache != null && _authInfoCache.IsAuthenticated) return _authInfoCache;
            _authInfoCache = await _authorizeApi.GetAuthorizationInfo();
            return _authInfoCache;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetAuthorizationInfo();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, _authInfoCache.UserName) }.Concat(_authInfoCache.ExposedClaims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}