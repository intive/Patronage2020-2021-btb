using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Blazor.Hosting;
using BTB.Client.Services.Implementations;
using BTB.Client.Services.Contracts;
using BTB.Domain.Policies;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Blazored.Toast;

namespace BTB.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
            });
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthorizeService, AuthorizeService>();
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton<Blazored.Modal.Services.IModalService, Blazored.Modal.Services.ModalService>();
            builder.Services.AddSingleton<IToastService, ToastService>();
            //builder.Services.AddSingleton<IHubProxy, HubProxy>();
            builder.Services.AddBlazoredToast();
            await builder.Build().RunAsync();
        }
    }
}