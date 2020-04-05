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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                    await SeedData(mediator);
                }
                catch (Exception e)
                {
                    // TODO log exception
                }
            }

            host.Run();
        }

        private static async Task SeedData(IMediator mediator)
        {
            int amount = Startup.Environment.IsDevelopment() ? 10 : 155;

            await mediator.Send(new LoadSymbolsCommand(), CancellationToken.None);

            foreach (TimestampInterval inter in Enum.GetValues(typeof(TimestampInterval)))
            {
                if (inter != TimestampInterval.Zero && inter != TimestampInterval.TwoWeeks)
                {
                    await mediator.Send(new LoadKlinesCommand() { KlineType = inter, Amount = amount, InitialCall = true });
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((context, logging) =>
                {
                    var env = context.HostingEnvironment;
                    var config = context.Configuration.GetSection("Logging");
                    logging.AddConfiguration(config);
                    logging.AddConsole();
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                    logging.AddFilter("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", LogLevel.Warning);
                })
                .UseStartup<Startup>();
    }
}