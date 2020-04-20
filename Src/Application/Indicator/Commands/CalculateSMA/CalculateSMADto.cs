using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Indicator.Commands.CalculateSMA
{
    public class CalculateSMADto
    {
        public int TimePeriod { get; set; }
        public List<decimal> Prices { get; set; }
    }
}
