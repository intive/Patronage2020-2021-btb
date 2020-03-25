using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.ValueObjects
{
    public class SimplePriceVO : SymbolVO
    {
        public decimal ClosePrice { get; set; }
        public decimal Volume { get; set; }
    }
}
