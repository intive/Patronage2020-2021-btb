using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using BTB.Application.Common.Exceptions;
using BTB.Application.Logs.Commands;
using BTB.Application.Logs.Queries.GetLogsFromDB;
using BTB.Application.Logs.Queries.GetLogsFromFileSystem;
using BTB.Server.Common.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BTB.Server.Controllers
{
    public class FileLogController : BaseController
    {
        private readonly FileLoggerConfig _config;
        private readonly ILogger _logger;

        public FileLogController(IOptions<FileLoggerConfig> config, ILogger<FileLogController> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        /// <summary>
        ///     Get current FileLogger config
        /// </summary>
        /// <returns>
        ///     Returns current config object.
        /// </returns>
        [HttpGet]
        [Route("config")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentConfiguration([FromQuery]string property)
        {
            var value = "";
            try
            {
                var myPropInfo = typeof(FileLoggerConfig).GetProperty(property);
                value = (string)myPropInfo.GetValue(_config, null);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Property {property} not found in current config!");
            }

            return Ok(value);
        }

        /// <summary>
        ///     Get application logs from server file system.
        /// </summary>
        /// <param name="logDate">Format: dd-MM-yyyy</param>
        /// <param name="logLevel">Log level</param>
        /// <param name="contains">Logs assigned to classes, which names contains this string will be returned.</param>
        /// <param name="limit">Limit returned logs. (order by latest)</param>
        /// <returns>
        ///     Returns .zip file with found log files on the server. (empty, if no logs were found)
        /// </returns>
        /// <response code="206">When returned at least one log.</response>
        [HttpGet]
        [Route("download")]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        public async Task<FileContentResult> GetLogsFromFileSystem([FromQuery]string logDate, [FromQuery]LogLevel logLevel, [FromQuery]string contains, [FromQuery]int limit)
        {
            var zipFileBytes = await Mediator.Send(new GetLogsFromFileSystemQuery { LogDate = logDate, LogLevel = logLevel, NameContains = contains, Limit = limit });
            return File(zipFileBytes.ToArray(), "application/zip", string.Concat(logDate + '_' + logLevel.ToString(), ".zip"));
        }

        /// <summary>
        ///     Removes application logs from server file system.
        /// </summary>
        /// <param name="logDate">Format: dd-MM-yyyy</param>
        /// <param name="logLevel">Log level</param>
        /// <param name="contains">Logs assigned to classes, which names contains this string will be returned.</param>
        /// <returns>
        ///     Removes all log files that meet the requirements;
        /// </returns>
        /// <response code="200">When removed at least one log.</response>
        /// <response code="404">When no logs were found.</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearLogsFromFileSystem(string logDate, LogLevel logLevel, string contains)
        {
            return Ok(await Mediator.Send(new ClearLogsFromFileSystemCommand() { LogDate = logDate, LogLevel = logLevel, NameContains = contains }));
        }
    }
}