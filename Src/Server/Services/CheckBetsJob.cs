using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.GamblePoints.Commands.AddValueToAllGamblePoints;
using BTB.Application.System.Commands.UpdateBetsCommand;
using BTB.Server.Common.CronGeneric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class CheckBetsJob : CronBaseJob
    {
        private IServiceProvider _serviceProvider;
        private ILogger<CheckBetsJob> _logger;

        private const decimal NumberOfPointsToAddDaily = 20;

        public CheckBetsJob(IScheduleConfig<CheckBetsJob> config, IServiceProvider services, ILoggerFactory loggerFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _serviceProvider = services;
            _logger = loggerFactory.CreateLogger<CheckBetsJob>();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public async override Task DoWork(CancellationToken cancellationToken)
        {

            using (var scope = _serviceProvider.CreateScope())
            {
                var betsManager = scope.ServiceProvider.GetRequiredService<IBetsManager>();

                try
                {
                    var checkBetsHandler = new CheckBetsCommandHandler(betsManager);
                    await checkBetsHandler.Handle(new CheckBetsCommand(), cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{nameof(CheckBetsJob)} An error occurred while updating bets.");
                    throw e;
                }
            }

        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
