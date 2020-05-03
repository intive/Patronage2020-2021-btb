using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Exceptions.BetsManager
{
    public class PriceRangeAboveLimitException : Exception
    {
        public PriceRangeAboveLimitException(string message = "")
            : base(message)
        {
        }
    }
}
