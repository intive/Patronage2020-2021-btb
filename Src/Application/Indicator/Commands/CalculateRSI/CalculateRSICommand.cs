using BTB.Application.Common.Interfaces;
using BTB.Domain.Common.Indicator;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Indicator.Commands.CalculateRSI
{
    public class CalculateRSICommand : IRequest<RSIIndicatorVm>
    {
        public List<decimal> ClosePrices { get; set; }
        public RSITimeframe Timeframe { get; set; }

        public class CalculateRSICommandHandler : IRequestHandler<CalculateRSICommand, RSIIndicatorVm>
        {
            private readonly IIndicator _indicator;

            public CalculateRSICommandHandler(IIndicator indicator)
            {
                _indicator = indicator;
            }

            public Task<RSIIndicatorVm> Handle(CalculateRSICommand request, CancellationToken cancellationToken)
            {
                var rsi = _indicator.CalculateRSIForGraph((int)request.Timeframe, request.ClosePrices);
                return Task.FromResult(new RSIIndicatorVm()
                {
                    RSI = rsi
                });
            }
        }
    }
}
