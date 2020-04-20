using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Indicator.Commands.CalculateSMA
{
    public class CalculateSMACommandHandler : IRequestHandler<CalculateSMACommand, SMAVm>
    {
        private readonly IIndicator _indicator;
        private readonly IMapper _mapper;

        public CalculateSMACommandHandler(IIndicator indicator, IMapper mapper)
        {
            _indicator = indicator;
            _mapper = mapper;
        }

        public Task<SMAVm> Handle(CalculateSMACommand request, CancellationToken cancellationToken)
        {
            var smaDto = _mapper.Map<CalculateSMADto>(request);
            if (smaDto.Prices.Count == 0)
            {
                return Task.FromResult(new SMAVm { SMA = new List<decimal>() });
            }
                
            if (smaDto.Prices.Count <= smaDto.TimePeriod)
            {
                throw new BadRequestException("Not enough prices to calculate SMA.");
            }
                
            var sma = _indicator.CalculateSMAForGraph(smaDto.TimePeriod, smaDto.Prices);
            return Task.FromResult(new SMAVm { SMA = sma });
        }
    }
}
