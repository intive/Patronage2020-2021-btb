using BTB.Client.Services.Contracts;
using BTB.Client.Components.Common;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net;
using System.IO;
using System;
using Blazored.Modal.Services;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace BTB.Client.Services.Implementations
{
    public class CustomHttpClient : ICustomHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenValidator _tokenValidator;
        private readonly IModalService _modalService;

        public CustomHttpClient(HttpClient httpClient, ITokenValidator tokenValidator, IModalService modalService)
        {
            _httpClient = httpClient;
            _tokenValidator = tokenValidator;
            _modalService = modalService;
        }

        public HttpRequestHeaders DefaultRequestHeaders => _httpClient.DefaultRequestHeaders;
        public Uri BaseAddress { get => _httpClient.BaseAddress; set => _httpClient.BaseAddress = value; }
        public long MaxResponseContentBufferSize { get => _httpClient.MaxResponseContentBufferSize; set => _httpClient.MaxResponseContentBufferSize = value; }
        public TimeSpan Timeout { get => _httpClient.Timeout; set => _httpClient.Timeout = value; }

        public async Task<bool> IsTokenExpiredAsync()
        {
            if (await _tokenValidator.DoesUserHaveATokenAsync() == true)
            {
                if (await _tokenValidator.IsTokenExpiredAsync() == true)
                {
                    return true;
                }
            }
            return false;
        }

        public HttpResponseMessage Logout()
        {
            var parameters = new ModalParameters();
            var options = new ModalOptions()
            {
                DisableBackgroundCancel = true,
                HideHeader = false,
                HideCloseButton = true
            };

            _modalService.Show<TokenExpirationLogout>("Your token has expired", parameters, options);

            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.DeleteAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.DeleteAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.DeleteAsync(requestUri);
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.DeleteAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri, completionOption);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri, completionOption);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri, completionOption, cancellationToken);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetAsync(requestUri, completionOption, cancellationToken);
        }

        public async Task<byte[]> GetByteArrayAsync(string requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetByteArrayAsync(requestUri);
        }

        public async Task<byte[]> GetByteArrayAsync(Uri requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetByteArrayAsync(requestUri);
        }

        public async Task<Stream> GetStreamAsync(string requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetStreamAsync(requestUri);
        }

        public async Task<Stream> GetStreamAsync(Uri requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetStreamAsync(requestUri);
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetStringAsync(requestUri);
        }

        public async Task<string> GetStringAsync(Uri requestUri)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.GetStringAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PatchAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PatchAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PatchAsync(requestUri, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PatchAsync(requestUri, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PostAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PostAsync(requestUri, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PostAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PostAsync(requestUri, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PutAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PutAsync(requestUri, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PutAsync(requestUri, content, cancellationToken);
        }

        public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.PutAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.SendAsync(request, completionOption);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.SendAsync(request, completionOption, cancellationToken);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            return await _httpClient.SendAsync(request, cancellationToken);
        }

        public async Task SendJsonAsync(HttpMethod method, string requestUri, object content)
        {
            if (await IsTokenExpiredAsync() == true)
            {
                Logout();
            }

            await _httpClient.SendJsonAsync(method, requestUri, content);
        }


    }
}
