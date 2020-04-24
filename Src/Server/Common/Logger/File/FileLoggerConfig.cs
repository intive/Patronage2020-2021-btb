using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BTB.Server.Common.Logger
{
    public class FileLoggerConfig : LoggerConfig
    {
        public LogLevel MinLogLevel { get; set; }
        public string MainDirectoryName { get; set; }
        public string FileExtension { get; set; }
        public int FileMaxSizeMb { get; set; }
        public int MaxLogSystemSizeMb { get; set; }
        public long MaxLogSystemSizeByte 
        { 
            get
            {
                return (long)MaxLogSystemSizeMb * 1024 * 1024 * 1024;
            } 
        }
        public string BaseDirectoryPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MainDirectoryName);
            }
        }
    }
}
