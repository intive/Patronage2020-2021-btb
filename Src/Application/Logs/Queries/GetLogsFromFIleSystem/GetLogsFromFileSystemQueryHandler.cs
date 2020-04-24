using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Logs.Queries.GetLogsFromFileSystem
{
    public class GetLogsFromFileSystemQueryHandler : IRequestHandler<GetLogsFromFileSystemQuery, List<byte>>
    {
        private readonly ILogFileService _logFileService;
        private readonly ILogger _logger;

        public GetLogsFromFileSystemQueryHandler(ILogFileService logFileService, ILogger<GetLogsFromFileSystemQueryHandler> logger)
        {
            _logFileService = logFileService;
            _logger = logger;
        }

        public async Task<List<byte>> Handle(GetLogsFromFileSystemQuery request, CancellationToken cancellationToken)
        {
            if (request.LogDate == null || string.IsNullOrEmpty(request.NameContains))
            {
                var e = new BadRequestException($"Uncorrect LogDate: {request.LogDate} or NameContains: {request.NameContains}");
                _logger.LogError(e, "Request error");
                throw e;
            }

            DateTime dateTime = DateTime.MinValue;
            bool process = true;

            try
            {
                dateTime = DateTime.ParseExact(request.LogDate, _logFileService.LogDateFormat, CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                _logger.LogError(e, $"Invalid date: {request.LogDate}, other params: LogLevel = {request.LogLevel}, Contains = {request.NameContains}, Limit = {request.Limit}");
                process = false;
            }

            var filePaths = _logFileService.GetLogFilePaths(dateTime, (LogLevel)request.LogLevel, request.NameContains);

            byte[] zipFileBytes = null;

            using (MemoryStream memStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memStream, ZipArchiveMode.Create, true))
                {
                    if (process)
                    {
                        foreach (string filePath in filePaths)
                        {
                            string parentDir = Directory.GetParent(filePath).FullName;
                            string parentParentDir = Directory.GetParent(parentDir).FullName;
                            string categoryName = parentDir.Substring(parentParentDir.Length + 1);

                            archive.CreateEntryFromFile(filePath, string.Concat(categoryName, _logFileService.FileExtension), CompressionLevel.Fastest);
                        }
                    }
                }
                zipFileBytes = memStream.ToArray();
            }

            return new List<byte>(zipFileBytes);
        }
    }
}
