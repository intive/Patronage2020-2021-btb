using BTB.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Interfaces
{
    public interface ILogFileService
    {
        string FileExtension { get; }
        string LogDateFormat { get; }

        long GetFreeSpaceLeftBytes();
        List<string> GetLogFilePaths(DateTime date, LogLevel logLevel, string nameContains);
        void SaveLogToFile(LogEntryVO logEntry);
        bool IsPossibleToAdd(long wantedFreeSpace);
        bool RemoveFileFromServer(string path);
    }
}
