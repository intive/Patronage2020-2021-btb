using BTB.Application.Alerts.Commands.CreateAlertCommand;
using BTB.Application.Common.Behaviours;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using BTB.Application.Binance;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net;
using System.Reflection;
using AutoMapper;
using MediatR;
using BTB.Application.Authorize.Commands.Register;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;

namespace BTB.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IBinanceClient, BinanceClient>();

            var sp = services.BuildServiceProvider();
            var settings = sp.GetService<IOptions<BinanceSettings>>();
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(
                    settings.Value.ApiKey,
                    settings.Value.SecretKey),
                LogVerbosity = LogVerbosity.Error,
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient<IValidator<CreateAlertCommand>, CreateAlertCommandValidator>();
            services.AddTransient<IValidator<UpdateAlertCommand>, UpdateAlertCommandValidator>();
            services.AddTransient<IValidator<CreateUserProfileCommand>, CreateUserProfileCommandValidator>();
            services.AddTransient<IValidator<UpdateUserProfileCommand>, UpdateUserProfileCommandValidator>();
            services.AddTransient<IValidator<RegisterCommand>, RegisterCommandValidator>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}