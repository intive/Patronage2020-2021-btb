using BTB.Domain.Common.Indicator;
using System.Collections.Generic;

namespace BTB.Application.Common.Interfaces
{
    public interface IIndicator
    {
        public List<decimal> CalculateRSIForGraph(int timeframe, List<decimal> closePrices);
        public List<decimal> CalculateSMAForGraph(int timePeriod, List<decimal> prices);
    }
}
