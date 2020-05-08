using BTB.Client.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using Blazored.LocalStorage;
using BTB.Domain.Extensions;
using System;

namespace BTB.Client.Services.Implementations
{
    public class TokenValidator : ITokenValidator
    {
        private readonly ILocalStorageService _localStorage;

        public TokenValidator(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<bool> DoesUserHaveATokenAsync()
        {
            string token = await GetTokenAsync();
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task<bool> IsTokenExpiredAsync()
        {
            string token = await GetTokenAsync();
            IEnumerable<Claim> claims = token.ParseClaimsFromJwt();
            Claim expirationClaim = claims.Where(c => c.Type.Equals("exp")).FirstOrDefault();

            if (expirationClaim == null)
            {
                return true;
            }

            var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim.Value));

            if (datetime.UtcDateTime <= DateTime.Now)
            {
                return true;
            }

            return false;
        }

        public async Task<string> GetTokenAsync()
        {
            string token = await _localStorage.GetItemAsync<string>("authToken");
            return token;
        }
    }
}
