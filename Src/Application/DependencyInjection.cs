using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using BTB.Application.UserManagement.Commands.TakeRoleFromUser;
using BTB.Application.UserManagement.Commands.GiveRoleToUser;
using BTB.Application.Alerts.Commands.CreateAlertCommand;
using BTB.Application.Logs.Queries.GetLogsFromFileSystem;
using BTB.Application.Authorize.Commands.ChangePassword;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.Indicator.Commands.CalculateSMA;
using BTB.Application.Indicator.Commands.CalculateRSI;
using BTB.Application.System.Commands.AddKlineCommand;
using BTB.Application.System.Commands.SendEmailCommand;
using BTB.Application.Authorize.Commands.SendResetLink;
using BTB.Application.Authorize.Commands.ResetPassword;
using BTB.Application.Authorize.Commands.Register;
using BTB.Application.UserProfile.Common;
using BTB.Application.Common.Behaviours;
using BTB.Application.Logs.Commands;
using BTB.Application.Alerts.Common;
using BTB.Application.Binance;
using BTB.Application.Logs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net;
using System.Reflection;
using FluentValidation;
using AutoMapper;
using MediatR;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Hubs;
using BTB.Application.ConditionDetectors;
using BTB.Application.ConditionDetectors.Between;
using BTB.Application.Bets.Common;

namespace BTB.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IBinanceClient, BinanceClient>();
            services.AddTransient<IBrowserNotificationHub, BrowserNotificationHub>();

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
            services.AddTransient<IValidator<CreateAlertCommand>, AlertRequestValidator>();
            services.AddTransient<IValidator<UpdateAlertCommand>, AlertRequestValidator>();
            services.AddTransient<IValidator<UpdateUserProfileCommand>, UserProfileInfoRequestValidator>();
            services.AddTransient<IValidator<RegisterCommand>, RegisterCommandValidator>();
            services.AddTransient<IValidator<SendEmailCommand>, SendEmailCommandValidator>();
            services.AddTransient<IValidator<AddKlineCommand>, AddKlineCommandValidator>();
            services.AddTransient<IValidator<CalculateRSICommand>, CalculateRSICommandValidator>();
            services.AddTransient<IValidator<CalculateSMACommand>, CalculateSMACommandValidator>();
            services.AddTransient<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
            services.AddTransient<IValidator<GiveRoleToUserCommand>, GiveRoleToUserCommandValidator>();
            services.AddTransient<IValidator<TakeRoleFromUserCommand>, TakeRoleFromUserCommandValidator>();
            services.AddTransient<IValidator<GetLogsFromFileSystemQuery>, LogRequestValidator>();
            services.AddTransient<IValidator<ClearLogsFromDBCommand>, LogRequestValidator>();
            services.AddTransient<IValidator<ClearLogsFromFileSystemCommand>, LogRequestValidator>();
            services.AddTransient<IValidator<SendResetLinkCommand>, SendResetLinkCommandValidator>();
            services.AddTransient<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidator>();
            services.AddTransient<IBetConditionDetector<BasicConditionDetectorParameters>, BetweenConditionDetector>();
            services.AddTransient<IValidator<BetRequestBase>, BetRequestValidator>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}