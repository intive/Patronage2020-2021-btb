﻿using Microsoft.AspNetCore.Components.Authorization;
using BTB.Client.Pages.Dto.Authorization;
using BTB.Client.Services.Contracts;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System;
using Blazored.LocalStorage;

namespace BTB.Client.Services.Implementations
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _identityAuthenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthorizeService(HttpClient httpClient, AuthenticationStateProvider identityAuthenticationStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _identityAuthenticationStateProvider = identityAuthenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task Login(LoginParametersDto loginParameters)
        {
            var stringContent = new StringContent(JsonSerializer.Serialize(loginParameters), Encoding.UTF8, "application/json");            
            var response = await _httpClient.PostAsync("api/Authorize/Login", stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            response.EnsureSuccessStatusCode();

            // TODO -> return userToken instead of string
            //var token = JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var token = await response.Content.ReadAsStringAsync();
            await _localStorage.SetItemAsync("authToken", token);
            ((IdentityAuthenticationStateProvider)_identityAuthenticationStateProvider).MarkUserAsAuthenticated(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((IdentityAuthenticationStateProvider)_identityAuthenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task Register(RegisterParametersDto registerParameters)
        {
            var stringContent = new StringContent(JsonSerializer.Serialize(registerParameters), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Authorize/Register", stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            response.EnsureSuccessStatusCode();
        }

    }
}