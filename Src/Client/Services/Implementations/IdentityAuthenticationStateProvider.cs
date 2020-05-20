using Microsoft.AspNetCore.Components.Authorization;
using BTB.Client.Services.Contracts;
using BTB.Domain.Extensions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BTB.Client.Services.Implementations
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ICustomHttpClient _httpClient;
        private readonly ITokenValidator _tokenValidator;

        public IdentityAuthenticationStateProvider(ICustomHttpClient httpClient, ITokenValidator tokenValidator)
        {
            _httpClient = httpClient;
            _tokenValidator = tokenValidator;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _tokenValidator.GetTokenAsync();
            var anonymousState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            if (await _tokenValidator.DoesUserHaveATokenAsync() == false)
            {
                return anonymousState;
            }

            if (await _tokenValidator.IsTokenExpiredAsync())
            {
                return anonymousState;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(savedToken.ParseClaimsFromJwt(), "jwt")));
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(token.ParseClaimsFromJwt(), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}