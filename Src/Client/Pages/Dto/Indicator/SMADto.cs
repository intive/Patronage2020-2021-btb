using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Pages.Dto.Indicator
{
    public class SMADto
    {
        public int TimePeriod { get; set; }
        public List<decimal> Prices { get; set; }
    }
}
