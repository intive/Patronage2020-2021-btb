using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.GamblePoints.Commands.AddValueToAllGamblePoints;
using BTB.Server.Common.CronGeneric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class DailyAddGamblePointsJob : CronBaseJob
    {
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;
        private ILogger<DailyAddGamblePointsJob> _logger;
        private string _expression;

        private const decimal NumberOfPointsToAddDaily = 20;

        public DailyAddGamblePointsJob(
            IScheduleConfig<DailyAddGamblePointsJob> config,
            IServiceProvider services,
            ILoggerFactory loggerFactory
            ) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _serviceProvider = services;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<DailyAddGamblePointsJob>();
            _expression = config.CronExpression;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public async override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(DailyAddGamblePointsJob)} just runned periodic work.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var gamblePointsManager = scope.ServiceProvider.GetRequiredService<IGamblePointManager>();

                try
                {
                    var addValueToAllPointsCommandHandler = new AddValueToAllGamblePointsCommandHandler(gamblePointsManager);
                    await addValueToAllPointsCommandHandler.Handle(new AddValueToAllGamblePointsCommand() { Amount = NumberOfPointsToAddDaily }, cancellationToken);
                }
                catch(ServiceUnavailableException e)
                {
                    _logger.LogError(e, $"{nameof(DailyAddGamblePointsJob)} encountered an error during adding values to all gamble points.");
                }            
            }

            _logger.LogInformation($"{nameof(DailyAddGamblePointsJob)} has finished periodic work.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning($"{nameof(DailyAddGamblePointsJob)} has been stopped of unknown reason");
            return base.StopAsync(cancellationToken);
        }
    }
}
