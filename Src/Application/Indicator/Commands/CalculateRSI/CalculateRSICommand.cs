using BTB.Domain.Common.Indicator;
using MediatR;
using System.Collections.Generic;

namespace BTB.Application.Indicator.Commands.CalculateRSI
{
    public class CalculateRSICommand : IRequest<RSIVm>
    {
        public List<string> ClosePrices { get; set; }
        public RSITimeframe? Timeframe { get; set; }

        public CalculateRSICommand()
        {
            ClosePrices = null;
            Timeframe = null;
        }
    }
}
