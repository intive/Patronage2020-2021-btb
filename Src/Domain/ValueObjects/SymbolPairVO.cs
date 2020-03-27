using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.ValueObjects
{
    public class SymbolPairVO
    {
        public string BuySymbolName { get; set; }
        public string SellSymbolName { get; set; }

        public string PairName
        {
            get
            {
                return string.Concat(BuySymbolName, SellSymbolName);
            }
        }
    }
}
