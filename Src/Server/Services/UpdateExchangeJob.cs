using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Application.System.Commands.Alerts.SendNotificationsCommand;
using BTB.Application.System.Commands.LoadData;
using BTB.Domain.Common;
using BTB.Server.Common.CronGeneric;
using MediatR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static BTB.Application.System.Commands.LoadData.LoadKlinesCommand;

namespace BTB.Server.Services
{
    public class UpdateExchangeJob : CronBaseJob
    {
        private readonly IMediator _mediator;
        private static List<TimestampInterval> _klinesToUpdate;
        private static bool _initialCall = true;

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

        public UpdateExchangeJob(IScheduleConfig<UpdateExchangeJob> config) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _mediator = Startup.Mediator;
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        public async override Task DoWork(CancellationToken cancellationToken)
        {
            int amount = 1;
            if (_initialCall)
            {
                _initialCall = false;
                //amount = 155;
            }

            //await LoadKlinesAsync(_klinesToUpdate, amount);
            await SendNotificationsAsync();
        }        

        private async Task SendNotificationsAsync()
        {
            await _mediator.Send(new SendNotificationsCommand());
        }

        private async Task LoadKlinesAsync(List<TimestampInterval> intervals, int amount)
        {
            foreach (TimestampInterval tst in intervals)
            {
                await _mediator.Send(new LoadKlinesCommand() { KlineType = tst, Amount = amount });
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
