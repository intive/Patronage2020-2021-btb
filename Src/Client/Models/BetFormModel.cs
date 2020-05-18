using BTB.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Models
{
    public class BetFormModel
    {
        public string SymbolPair { get; set; }
        public decimal Points { get; set; }
        public decimal LowerPriceThreshold { get; set; }
        public decimal UpperPriceThreshold { get; set; }
        public string RateType { get; set; }
        public string TimeInterval { get; set; }

        public BetFormModel()
        {
        }

        public BetFormModel(BetVO vo)
        {
            SymbolPair = vo.SymbolPair;
            Points = vo.Points;
            LowerPriceThreshold = vo.LowerPriceThreshold;
            UpperPriceThreshold = vo.UpperPriceThreshold;
            RateType = vo.RateType.ToString();
            TimeInterval = vo.TimeInterval.ToString();
        }
    }
}
