using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common.CronGeneric
{
    public interface IScheduleConfig<T>
    {        
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
