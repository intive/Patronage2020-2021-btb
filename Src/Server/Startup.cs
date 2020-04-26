using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Server.Common.CronGeneric;
using BTB.Server.Common.Swagger;
using BTB.Application.Common;
using BTB.Application.Binance;
using BTB.Server.Services;
using BTB.Infrastructure;
using BTB.Application;
using BTB.Persistence;
using Newtonsoft.Json.Converters;
using System.Net.Mime;
using System.Linq;
using System;
using MediatR;
using BTB.Application.Common;
using BTB.Server.Common.Logger;
using BTB.Server.Common.Logger.Database;
using Microsoft.Extensions.Logging;
using BTB.Application.Common.Behaviours;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BTB.Server
{

    public class Startup
    {
        public static Dictionary<string, Exception> ConfigureServicesExceptions = new Dictionary<string, Exception>();
        public static Dictionary<string, Exception> ConfigureExceptions = new Dictionary<string, Exception>();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public static IWebHostEnvironment Environment { get; internal set; }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {

                services.Configure<FileLoggerConfig>(Configuration.GetSection("FileLoggerConfig"));
                services.Configure<DatabaseLoggerConfig>(Configuration.GetSection("DatabaseLoggerConfig"));
                services.Configure<FileLoggerConfig>(Configuration.GetSection("SharedLoggerConfig"));
                services.Configure<DatabaseLoggerConfig>(Configuration.GetSection("SharedLoggerConfig"));

                services.AddControllers(options =>
                {
                    options.Filters.Add(new HttpResponseExceptionFilter());
                    options.Filters.Add(new ValidationExceptionFilter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var result = new BadRequestObjectResult(context.ModelState);
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        return result;
                    };
                });

                services.Configure<BinanceSettings>(Configuration.GetSection("BinanceSettings"));

                services.AddApplication();
                services.AddInfrastructure(Configuration, Environment);
                services.AddPersistence(Configuration);

                services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

                services.AddControllers().AddNewtonsoftJson();

                services.AddResponseCompression(opts =>
                {
                    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "application/octet-stream" });
                });

            services.AddSwaggerDocumentation();

                services.AddMvc(options =>
                {
                    options.Filters.Add(new GlobalExceptionFilter(new LoggerFactory()));
                }
                    ).AddNewtonsoftJson(options =>
                        options.SerializerSettings.Converters.Add(new StringEnumConverter()));
                services.AddResponseCompression(opts =>
                {
                    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "application/octet-stream" });
                });

                services.AddScoped<IEmailService, EmailService>();
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddTransient<IPasswordManager, PasswordManager>();
                services.AddScoped<IBTBBinanceClient, BinanceMiddleService>();
                services.AddScoped<ILogFileService, LogFileSystemService>();

                services.AddCronJob<UpdateExchangeJob>(c =>
                {
                    c.TimeZoneInfo = TimeZoneInfo.Local;
                    c.CronExpression = @"*/5 * * * *";
                });

                services.AddCronJob<UpdateDatabaseLogsJob>(c =>
                {
                    c.TimeZoneInfo = TimeZoneInfo.Local;
                    c.CronExpression = @"* * * * *";
                }); ;

                services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));
            }
            catch (Exception e)
            {
                ConfigureServicesExceptions[e.ToString()] = e;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                app.UseResponseCompression();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseBlazorDebugging();
                }

            app.UseSwaggerDocumentation();

                app.UseStaticFiles();
                app.UseClientSideBlazorFiles<Client.Program>();

                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapFallbackToClientSideBlazor<Client.Program>("index.html");
                });
            }
            catch (Exception e)
            {
                ConfigureExceptions[e.ToString()] = e;
            }
            
        }
    }
}