using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace BTB.Application.Indicator.Commands.CalculateRSI
{
    public class CalculateRSICommandHandler : IRequestHandler<CalculateRSICommand, RSIVm>
    {
        private readonly IIndicator _indicator;
        private readonly IMapper _mapper;

        public CalculateRSICommandHandler(IIndicator indicator, IMapper mapper)
        {
            _indicator = indicator;
            _mapper = mapper;
        }

        public Task<RSIVm> Handle(CalculateRSICommand request, CancellationToken cancellationToken)
        {
            var rsiDto = _mapper.Map<CalculateRSIDto>(request);
            if (rsiDto.ClosePrices.Count == 0)
            {
                return Task.FromResult(new RSIVm { RSI = new List<decimal>() });
            }

            var timeframe = (int)rsiDto.Timeframe;
            if (timeframe >= rsiDto.ClosePrices.Count)
            {
                throw new BadRequestException("Not enough prices to calculate RSI for given Timeframe.");
            }
            var rsi = _indicator.CalculateRSIForGraph(timeframe, rsiDto.ClosePrices);
            return Task.FromResult(new RSIVm { RSI = rsi });
        }
    }
}
