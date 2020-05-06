using BTB.Application.Common.Interfaces;
using BTB.Application.System.Commands.UpdateBetsCommand.CheckActiveBetsCommand;
using BTB.Server.Common.CronGeneric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class CheckActiveBetsJob : CronBaseJob
    {
        private IServiceProvider _serviceProvider;
        private ILogger<CheckActiveBetsJob> _logger;

        private const decimal NumberOfPointsToAddDaily = 20;

        public CheckActiveBetsJob(IScheduleConfig<CheckActiveBetsJob> config, IServiceProvider services, ILoggerFactory loggerFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _serviceProvider = services;
            _logger = loggerFactory.CreateLogger<CheckActiveBetsJob>();
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
                    var checkBetsHandler = new CheckActiveBetsCommandHandler(betsManager);
                    await checkBetsHandler.Handle(new CheckActiveBetsCommand(), cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{nameof(CheckActiveBetsJob)} An error occurred while updating bets.");
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
