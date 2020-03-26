using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.ValueObjects
{
    public class SimplePriceVO : SymbolPairVO
    {
        public decimal ClosePrice { get; set; }
        public decimal Volume { get; set; }
    }
}
