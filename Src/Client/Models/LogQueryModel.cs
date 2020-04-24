using BTB.Client.Pages.Logs.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Models
{
    public class LogQueryModel
    {
        public LogQueryModel()
        {
            LogLevel = LogLevel.None;
        }

        public LogLevel LogLevel { get; set; }
        public string Contains { get; set; }
        public int Limit { get; set; }

        public List<DatePart> DateOrder { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public char DateSeparator { get; set; }

        public string LogDate 
        {
            get
            {
                string result = "";

                if (
                    string.IsNullOrEmpty(Day) ||
                    string.IsNullOrEmpty(Month) ||
                    string.IsNullOrEmpty(Year)
                    )
                    return string.Empty;

                for (int i=0; i<3; i++)
                {
                    switch (DateOrder[i])
                    {
                        case DatePart.Day:
                            result += Day;
                            break;
                        case DatePart.Month:
                            result += Month;
                            break;
                        case DatePart.Year:
                            result += Year;
                            break;
                    }

                    if (i != DateOrder.Count-1)
                    {
                        result += DateSeparator;
                    }
                }

                return result;
            } 
        }
    }
}
