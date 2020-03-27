using BTB.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BTB.Infrastructure.Stock.Indicator
{
    public class Indicator : IIndicator
    {
        public List<decimal> CalculateRSIForGraph(int timeframe, List<decimal> closePrices)
        {
            decimal change;
            List<decimal> upwardMovement = new List<decimal>();
            List<decimal> downwardMovement = new List<decimal>();

            closePrices.Reverse();
            for (int i = 0; i < closePrices.Count - 1; i++)
            {
                change = closePrices[i + 1] - closePrices[i];
                if (change >= 0)
                {
                    upwardMovement.Add(change);
                    downwardMovement.Add(0);
                }
                else
                {
                    upwardMovement.Add(0);
                    downwardMovement.Add(Math.Abs(change));
                }
            }
            List<decimal> RSI = new List<decimal>();

            // first averages are calculated differently then the others
            decimal previousAvgUpwardMovement = upwardMovement.Take(timeframe).Average();
            decimal previousAvgDownwardMovement = downwardMovement.Take(timeframe).Average();
            RSI.Add(previousAvgDownwardMovement == 0 ? 100 : 100 - 100 / (previousAvgUpwardMovement / previousAvgDownwardMovement + 1));

            decimal currAvgUpwardMovement;
            decimal currAvgDownwardMovement;

            int intervalCount = closePrices.Count - timeframe - 1;
            for (int i = 1; i < intervalCount; i++) // starts with one cause first elements have already benn calculated
            {
                currAvgUpwardMovement = (previousAvgUpwardMovement * (timeframe - 1) + upwardMovement.Skip(timeframe).ToList()[i]) / timeframe;
                currAvgDownwardMovement = (previousAvgDownwardMovement * (timeframe - 1) + downwardMovement.Skip(timeframe).ToList()[i]) / timeframe;

                RSI.Add(currAvgDownwardMovement == 0? 100 : 100 - 100 / (currAvgUpwardMovement / currAvgDownwardMovement + 1));

                previousAvgUpwardMovement = currAvgUpwardMovement;
                previousAvgDownwardMovement = currAvgDownwardMovement;
            }
            return RSI;
        }

        public List<decimal> CalculateSMAForGraph(int timePeriod, List<decimal> prices)
        {
            int resultsCount = prices.Count - timePeriod;
            List<decimal> SMA = new List<decimal>();

            for (int i = 0; i < resultsCount; i++)
            {
                SMA.Add(prices.Skip(i).Take(timePeriod).Average());
            }

            SMA.Reverse();
            return SMA;
        }

    }
}
