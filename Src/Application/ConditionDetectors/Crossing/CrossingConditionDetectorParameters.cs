using BTB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.ConditionDetectors.Crossing
{
    public class CrossingConditionDetectorParameters
    {
        public Kline NewKline { get; set; }
        public Kline OldKline { get; set; }
    }
}
