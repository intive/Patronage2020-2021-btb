using BTB.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Indicator.Commands.CalculateSMA
{
    public class CalculateSMACommand : IRequest<SMAIndicatorVm>
    {
        public int TimePeriod { get; set; }
        public List<decimal> Prices { get; set; }

        public class CalculateSMACommandHandler : IRequestHandler<CalculateSMACommand, SMAIndicatorVm>
        {
            private readonly IIndicator _indicator;

            public CalculateSMACommandHandler(IIndicator indicator)
            {
                _indicator = indicator;
            }

            public Task<SMAIndicatorVm> Handle(CalculateSMACommand request, CancellationToken cancellationToken)
            {
                var sma = _indicator.CalculateSMAForGraph(request.TimePeriod, request.Prices);
                return Task.FromResult(new SMAIndicatorVm()
                {
                    SMA = sma
                });
            }
        }
    }
}
