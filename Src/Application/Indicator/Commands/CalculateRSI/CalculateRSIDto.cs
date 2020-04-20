using BTB.Domain.Common.Indicator;
using System.Collections.Generic;

namespace BTB.Application.Indicator.Commands.CalculateRSI
{
    public class CalculateRSIDto
    {
        public List<decimal> ClosePrices { get; set; }
        public RSITimeframe Timeframe { get; set; }
    }
}
