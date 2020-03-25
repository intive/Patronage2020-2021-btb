using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Common
{
    public enum TimestampInterval
    {
        Zero = 0,
        FiveMin = 5 * 60,
        FifteenMin = 3 * FiveMin,
        OneHour = 12 * FiveMin,
        TwoHours = 2 * OneHour,
        FourHours = 2 * TwoHours,
        TwelveHours = 3 * FourHours,
        OneDay = 24 * OneHour,
        TwoWeeks = 14 * OneDay
    }
}
