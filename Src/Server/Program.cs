using Binance.Net.Objects;
using BTB.Application.Common.Models;
using BTB.Application.System.Commands.LoadData;
using BTB.Application.System.Commands.SeedSampleData;
using BTB.Domain.Common;
using BTB.Persistence;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            const string blazorConfigPath = @"/app/bin/Debug/netcoreapp3.1/BTB.Client.blazor.config";
            var blazorConfig = File.ReadAllText(blazorConfigPath);
            blazorConfig = Regex.Replace(blazorConfig, @"[a-zA-Z]:\\.+?\\Client\\", "/Client/")
                .Replace('\\', '/');
            File.WriteAllText(blazorConfigPath, blazorConfig);
#endif
            
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var apiContext = services.GetRequiredService<BTBDbContext>();
                    apiContext.Database.Migrate();

                    var mediator = services.GetRequiredService<IMediator>();
                    await mediator.Send(new SeedSampleDataCommand(), CancellationToken.None);
                    await mediator.Send(new LoadSymbolsCommand(), CancellationToken.None);
                }
                catch (Exception e)
                {
                    // TODO log exception
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .UseStartup<Startup>();
    }
}