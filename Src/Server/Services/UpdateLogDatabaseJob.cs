using BTB.Application.Common.Interfaces;
using BTB.Application.Logs.Commands;
using BTB.Server.Common.CronGeneric;
using BTB.Server.Common.Logger.Database;
using Cronos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class UpdateDatabaseLogsJob : CronBaseJob
    {
        private readonly ILogger<UpdateDatabaseLogsJob> _logger;

        public UpdateDatabaseLogsJob(
                IScheduleConfig<UpdateDatabaseLogsJob> config, 
                IServiceProvider services,
                ILogger<UpdateDatabaseLogsJob> logger
            ) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            return LoadLogsToDatabase(cancellationToken);
        }

        private async Task LoadLogsToDatabase(CancellationToken cancellationToken)
        {
            await DatabaseLoggerProvider.ForceLog();
            _logger.LogInformation($"{nameof(UpdateDatabaseLogsJob)} has pushed all local logs to database.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning($"{nameof(UpdateDatabaseLogsJob)} has been stopped of unknown reason");
            return base.StopAsync(cancellationToken);
        }
    }
}
