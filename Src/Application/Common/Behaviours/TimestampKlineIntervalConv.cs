using AutoMapper;
using Binance.Net.Objects;
using BTB.Application.Common.Exceptions;
using BTB.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Behaviours
{
    public abstract class TimestampKlineIntervalConv
    {
        public static KlineInterval GetKlineInterval(TimestampInterval timestampInterval)
        {
            switch (timestampInterval)
            {
                case TimestampInterval.FiveMin:
                    {
                        return KlineInterval.FiveMinutes;
                    }
                case TimestampInterval.FifteenMin:
                    {
                        return KlineInterval.FifteenMinutes;
                    }
                case TimestampInterval.OneHour:
                    {
                        return KlineInterval.OneHour;
                    }
                case TimestampInterval.TwoHours:
                    {
                        return KlineInterval.TwoHour;
                    }
                case TimestampInterval.FourHours:
                    {
                        return KlineInterval.FourHour;
                    }
                case TimestampInterval.TwelveHours:
                    {
                        return KlineInterval.TwelveHour;
                    }
                case TimestampInterval.OneDay:
                    {
                        return KlineInterval.OneDay;
                    }
            }

            throw new BadRequestException("This type of kline doesn't exist in Database!");
        }

        public static TimestampInterval GetTimestampInterval(KlineInterval klineInterval)
        {
            switch (klineInterval)
            {
                case KlineInterval.FiveMinutes:
                    {
                        return TimestampInterval.FiveMin;
                    }
                case KlineInterval.FifteenMinutes:
                    {
                        return TimestampInterval.FifteenMin;
                    }
                case KlineInterval.OneHour:
                    {
                        return TimestampInterval.OneHour;
                    }
                case KlineInterval.TwoHour:
                    {
                        return TimestampInterval.TwoHours;
                    }
                case KlineInterval.FourHour:
                    {
                        return TimestampInterval.FourHours;
                    }
                case KlineInterval.TwelveHour:
                    {
                        return TimestampInterval.TwelveHours;
                    }
                case KlineInterval.OneDay:
                    {
                        return TimestampInterval.OneDay;
                    }
            }

            return TimestampInterval.Zero;
        }
    }
}
