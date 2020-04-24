using BTB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.ValueObjects
{
    public class LogEntryVO : LogEntry
    {
        public LogEntryVO(LogEntry logEntry)
        {
            Id = logEntry.Id;
            UserName = logEntry.UserName;
            HostName = logEntry.HostName;
            TimeStampUtc = logEntry.TimeStampUtc;
            Category = logEntry.Category;
            Level = logEntry.Level;
            Text = logEntry.Text;
            Exception = logEntry.Exception;
            StackTrace = logEntry.StackTrace;
        }

        public LogEntryVO(Exception e, bool printStackTrace)
        {
            TimeStampUtc = DateTime.UtcNow;
            UserName = Environment.UserName;
            HostName = StaticHostName;

            SetStackTrace(e, printStackTrace);
        }

        private void SetStackTrace(Exception e, bool printStackTrace)
        {         
            if (printStackTrace && e != null)
            {
                StackTrace = e.StackTrace ?? string.Empty;
                Exception = e.ToString();
            }
            else
            {
                Exception = string.Empty;
                StackTrace = string.Empty;
            }
        }

        static public readonly string StaticHostName = System.Net.Dns.GetHostName();

        public string GetString()
        {
            return new string(
                    ToStringSave(Level) +
                    ToStringSave(UserName) +
                    ToStringSave(TimeStampUtc) +
                    ToStringSave(Category) +
                    ToStringSave(Text) +
                    ToStringSave(Exception) +
                    ToStringSave(StackTrace)
                );
        }

        private string ToStringSave(object o)
        {
            try
            {
                return o.ToString() + " ";
            }
            catch
            {
                return " ";
            }
        }
    }
}
