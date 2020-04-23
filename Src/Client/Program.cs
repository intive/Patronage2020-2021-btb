using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Blazor.Hosting;
using BTB.Client.Services.Implementations;
using BTB.Client.Services.Contracts;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace BTB.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthorizeService, AuthorizeService>();
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton<Blazored.Modal.Services.IModalService, Blazored.Modal.Services.ModalService>();
            await builder.Build().RunAsync();
        }
    }
}