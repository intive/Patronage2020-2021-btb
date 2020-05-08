using BTB.Application.System.Commands.SendEmailNotificationsCommand;
using BTB.Application.System.Commands.LoadData;
using BTB.Domain.Common;
using BTB.Server.Common.CronGeneric;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using BTB.Application.Common.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BTB.Application.Common.Interfaces;
using static BTB.Application.System.Commands.LoadData.LoadKlinesCommand;
using Binance.Net.Interfaces;

namespace BTB.Server.Services
{
    public class UpdateExchangeJob : CronBaseJob
    {
        private static List<TimestampInterval> _klinesToUpdate;
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;
        private ILogger<UpdateExchangeJob> _logger;
        private string _expression;

        static UpdateExchangeJob()
        {
            _klinesToUpdate = new List<TimestampInterval>()
            {
                TimestampInterval.FiveMin,
                TimestampInterval.FifteenMin,
                TimestampInterval.OneHour,
                TimestampInterval.TwoHours,
                TimestampInterval.FourHours,
                TimestampInterval.TwelveHours,
                TimestampInterval.OneDay
            };
        }

        public UpdateExchangeJob(
                IScheduleConfig<UpdateExchangeJob> config,
                IServiceProvider services,
                ILoggerFactory loggerFactory
            ) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _serviceProvider = services;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<UpdateExchangeJob>();
            _expression = config.CronExpression;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public async override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(UpdateExchangeJob)} just runned periodic work.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IBTBDbContext>();
                var binanceClient = scope.ServiceProvider.GetRequiredService<IBinanceClient>();
                var hub = scope.ServiceProvider.GetRequiredService<IBrowserNotificationHub>();

                foreach (var interval in _klinesToUpdate)
                {
                    try
                    {
                        var loadKlinesHandler = new LoadKlinesCommandHandler(binanceClient, context, _loggerFactory);
                        await loadKlinesHandler.Handle(new LoadKlinesCommand() { KlineType = interval, Amount = 1 }, cancellationToken);
                    }
                    catch (ServiceUnavailableException e)
                    {
                        _logger.LogError(e, $"{nameof(UpdateExchangeJob)} encountered an error during loading Klines {interval} to database.");
                    }
                }

                var email = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var sendEmailHandler = new SendEmailNotificationsCommandHandler(context, email, hub);
                await sendEmailHandler.Handle(new SendEmailNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin }, cancellationToken);
            }

            _logger.LogInformation($"{nameof(UpdateExchangeJob)} has finished periodic work.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning($"{nameof(UpdateExchangeJob)} has been stopped of unknown reason");
            return base.StopAsync(cancellationToken);
        }
    }
}
