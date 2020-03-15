using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    internal class UpdateExchangeService : BackgroundService
    {
        private int executionCount = 0;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                executionCount++;
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
