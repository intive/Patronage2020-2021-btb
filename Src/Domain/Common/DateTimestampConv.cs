using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Common
{
    public abstract class DateTimestampConv
    {
        public static long GetTimestamp(DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }

        public static DateTime GetDateTime(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }
    }
}
