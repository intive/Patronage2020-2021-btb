using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Application.System.Commands.LoadData;
using BTB.Application.System.Commands.SeedSampleData;
using BTB.Domain.Common;
using BTB.Domain.ValueObjects;
using BTB.Persistence;
using BTB.Server.Common;
using BTB.Server.Common.Logger;
using BTB.Server.Common.Logger.Database;
using BTB.Server.Services;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static List<Exception> ProgramExceptions { get; set; } = new List<Exception>();

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
            ServiceProvider = host.Services;

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

                    var logger = host.Services.GetRequiredService<ILogger<Program>>();
                }
                catch (Exception e)
                {
                    ProgramExceptions.Add(e);
                }
            }

            var app = AppDomain.CurrentDomain;
            app.FirstChanceException += InspectException;

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

        private static void InspectException(object sender, FirstChanceExceptionEventArgs e)
        {
            var exception = (Exception)e.Exception;

            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger logger = loggerFactory.CreateLogger("First.Chance.Exception");
            logger.LogError(exception, "Exception was catched, now it will go inside the app");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((context, logging) =>
                {
                    var env = context.HostingEnvironment;
                    
                    var logCfg = context.Configuration.GetSection("Logging");
                    logging.AddConfiguration(logCfg);

                    logging.ClearProviders();
                    logging.AddConsole();

                    var sharedLogCfg = context.Configuration.GetSection("SharedLoggerConfig");
                    var fileLogCfg = context.Configuration.GetSection(nameof(FileLoggerConfig));
                    var dbLogCfg = context.Configuration.GetSection(nameof(DatabaseLoggerConfig));

                    var fileConfig = new FileLoggerConfig();
                    var dbConfig = new DatabaseLoggerConfig();

                    fileLogCfg.Bind(fileConfig);
                    dbLogCfg.Bind(dbConfig);
                    sharedLogCfg.Bind(fileConfig);
                    sharedLogCfg.Bind(dbConfig);

                    logging.AddFileLogger(fileConfig);                 
                    logging.AddDatabaseLogger(dbConfig);                    

                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                    logging.AddFilter("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", LogLevel.Warning);
                    logging.AddFilter("BTB", LogLevel.Information);
                })
                .UseStartup<Startup>();
    }
}