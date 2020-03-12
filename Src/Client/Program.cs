using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BTB.Client.States;
using Microsoft.AspNetCore.Components.Authorization;
using BTB.Client.Services.Implementations;
using BTB.Client.Services.Contracts;

namespace BTB.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<IdentityAuthenticationStateProvider>());
            builder.Services.AddScoped<IAuthorizeApi, AuthorizeApi>();
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton<Blazored.Modal.Services.IModalService, Blazored.Modal.Services.ModalService>();
            await builder.Build().RunAsync();
        }
    }
}