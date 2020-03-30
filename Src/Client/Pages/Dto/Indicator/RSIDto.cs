using BTB.Domain.Common.Indicator;
using System.Collections.Generic;

namespace BTB.Client.Pages.Dto.Indicator
{
    public class RSIDto
    {
        public List<decimal> ClosePrices { get; set; }
        public RSITimeframe Timeframe { get; set; }
    }
}
