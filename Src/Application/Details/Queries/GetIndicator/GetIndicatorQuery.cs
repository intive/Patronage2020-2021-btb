using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using MediatR;
using BTB.Domain.Common.Indicator;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Exceptions;

namespace BTB.Application.Details.Queries.GetIndicator
{
    public class GetIndicatorQuery : IRequest<IndicatorValuesVm>
    {
        public List<decimal> ClosePrices { get; set; }
        public RSITimeframe Timeframe { get; set; }
        //public IndicatorType IndicatorType { get; set; }

        public class GetIndicatorQueryHandler : IRequestHandler<GetIndicatorQuery, IndicatorValuesVm>
        {
            private readonly IIndicator _indicator;

            public GetIndicatorQueryHandler(IIndicator indicator)
            {
                _indicator = indicator;
            }

            public Task<IndicatorValuesVm> Handle(GetIndicatorQuery request, CancellationToken cancellationToken)
            {
                var list = _indicator.CalculateRSIForGraph((int)request.Timeframe, request.ClosePrices);

                //switch (request.IndicatorType)
                //{
                //    case IndicatorType.RSI:
                //        var list = _indicator.CalculateRSIForGraph((int)request.Timeframe, request.ClosePrices);
                //        break;
                //}


                throw new BadRequestException();
            }
        }
    }
}
