using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Application.System.Commands.LoadData;
using BTB.Domain.Common;
using BTB.Server.Common.CronGeneric;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static BTB.Application.System.Commands.LoadData.LoadKlinesCommand;

namespace BTB.Server.Services
{
    public class UpdateExchangeJob : CronBaseJob
    {
        private IBinanceClient _client;
        private IBTBDbContext _context;

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
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        public async override Task DoWork(CancellationToken cancellationToken)
        {
            _client = new BinanceClient();
            _context = Startup.BTBDbContext;
            var handler = new LoadKlinesCommandHandler(_client, _context);

            int amount = 1;
            if (_initialCall)
            {
                _initialCall = false;
                //amount = 155;
            }
            //await LoadKlines(_klinesToUpdate, amount);

            var emailAlertsHandler = new EmailAlertsHandler(_context);
            await emailAlertsHandler.Handle();
        }        

        private async Task LoadKlines(List<TimestampInterval> intervals, int amount)
        {
            var handler = new LoadKlinesCommandHandler(_client, _context);

            foreach (TimestampInterval tst in intervals)
            {
                await handler.Handle(new LoadKlinesCommand() { KlineType = tst, Amount = amount }, CancellationToken.None);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
