using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using BTB.Server.Common.Logger;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BTB.Server.Services
{
    public class LogFileSystemService : ILogFileService
    {
        private readonly FileLoggerConfig _logConfig;

        public LogFileSystemService(FileLoggerConfig logConfig)
        {
            _logConfig = logConfig;
        }

        public LogFileSystemService(IOptions<FileLoggerConfig> options) : this(options.Value)
        {
        }

        public string FileExtension
        {
            get
            {
                return _logConfig.FileExtension;
            }
        }

        public string LogDateFormat
        {
            get
            {
                return _logConfig.LogDateFormat;
            }
        }

        public void SaveLogToFile(LogEntryVO log)
        {
            List<string> directories = new List<string>();
            string path = _logConfig.BaseDirectoryPath;

            if (_logConfig.LogDateFormat == null)
            {
                return;
            }

            directories.Add(CreatePathFromDate(log.TimeStampUtc));
            directories.Add(log.Category);
            string fileName = string.Concat(log.Level.ToString(), _logConfig.FileExtension);

            Directory.CreateDirectory(path);

            foreach (string dir in directories)
            {
                path = Path.Combine(path, dir);
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, fileName);

            if (!File.Exists(path))
            {
                try
                {
                    File.Create(path).Dispose();
                }
                catch (Exception e)
                {
                    return;
                }
            }

            string message = log.GetString();
            long wantedFreeSpaceByte = message.Length;

            if (IsPossibleToAdd(wantedFreeSpaceByte))
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read))
                {
                    stream.Seek(0, SeekOrigin.End);
                    stream.Write(Encoding.ASCII.GetBytes(message));
                    stream.Write(Encoding.ASCII.GetBytes("\n"));
                    stream.Dispose();
                }
            }
            else
            {
                //todo throw exception or something
            }
        }

        public bool IsPossibleToAdd(long wantedFreeSpace)
        {
            Directory.CreateDirectory(_logConfig.BaseDirectoryPath);
            DirectoryInfo info = new DirectoryInfo(_logConfig.BaseDirectoryPath);
            long currentSizeByte = GetDirectorySize(info);

            if (currentSizeByte + wantedFreeSpace > _logConfig.MaxLogSystemSizeByte)
            {
                return false;
            }

            return true;
        }

        private long GetDirectorySize(DirectoryInfo directory)
        {
            long sizeByte = 0;

            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                sizeByte += file.Length;
            }

            DirectoryInfo[] directories = directory.GetDirectories();
            foreach (DirectoryInfo dir in directories)
            {
                sizeByte += GetDirectorySize(dir);
            }

            return sizeByte;
        }

        public long GetFreeSpaceLeftBytes()
        {
            return _logConfig.MaxLogSystemSizeByte;
        }

        public List<string> GetLogFilePaths(DateTime date, LogLevel logLevel, string nameContains)
        {
            List<string> result = new List<string>();

            if (DirExist(_logConfig.BaseDirectoryPath))
            {
                string datePath = CreatePathFromDate(date);
                if (DirExist(datePath))
                {
                    DirectoryInfo pathDir = new DirectoryInfo(datePath);
                    DirectoryInfo[] categoryDirs = pathDir.GetDirectories();

                    foreach (DirectoryInfo dir in categoryDirs)
                    {
                        if (dir.FullName.Contains(nameContains))
                        {
                            string logFilePath = Path.Combine(dir.FullName, string.Concat(logLevel.ToString(), _logConfig.FileExtension));
                            if (File.Exists(logFilePath))
                            {
                                result.Add(logFilePath);
                            }
                        }
                                             
                    }                    
                }
            }

            return result;
        }

        private string CreatePathFromDate(DateTime date)
        {

            return Path.Combine(_logConfig.BaseDirectoryPath, date.ToString(_logConfig.LogDateFormat)
                .Replace(Path.DirectorySeparatorChar, '-')
                .Replace(Path.AltDirectorySeparatorChar, '-'));
        }

        private bool DirExist(string path)
        {
            return Directory.Exists(path);
        }

        public bool RemoveFileFromServer(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
