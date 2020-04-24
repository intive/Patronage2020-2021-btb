using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public abstract class CronBaseJob : IHostedService
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private long InitialDelayMilis = 2 * 60 * 1000;

        protected CronBaseJob(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
        }

        protected async Task SkipAndFire(CancellationToken cancellationToken)
        {
            _timer = null;
            await DoWork(cancellationToken);
            await ScheduleJob(cancellationToken);
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer(InitialDelayMilis);
            _timer.Elapsed += async (sender, args) => { await ScheduleJob(cancellationToken); };
            _timer.Start();
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                _timer?.Stop();
                _timer?.Dispose();
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };

                _timer.Start();
            }
            await Task.CompletedTask;
        }        

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);  // do the work
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }
    }
}
