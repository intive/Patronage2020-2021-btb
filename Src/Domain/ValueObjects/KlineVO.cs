using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.ValueObjects
{
    public class KlineVO : SimplePriceVO
    {        
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal HighestPrice { get; set; }
    }
}
