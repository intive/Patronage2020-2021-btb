using System;
using System.Threading.Tasks;
using BTB.Application.Logs.Commands;
using BTB.Application.Logs.Queries.GetLogsFromDB;
using BTB.Server.Common.Logger.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BTB.Domain.Extensions;
using System.Collections.Generic;
using BTB.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using BTB.Domain.Policies;

namespace BTB.Server.Controllers
{
    [Authorize(Policy = Policies.IsAdmin)]
    public class DBLogController : BaseController
    {
        private readonly DatabaseLoggerConfig _config;
        private readonly ILogger _logger;

        public DBLogController(IOptions<DatabaseLoggerConfig> config, ILogger<DBLogController> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        /// <summary>
        ///     Get current DatabaseLogger config
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
                var myPropInfo = typeof(DatabaseLoggerConfig).GetProperty(property);
                value = (string)myPropInfo.GetValue(_config, null);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Property {property} not found in current config!");
            }

            return Ok(value);
        }

        /// <summary>
        ///     Get application logs from database.
        /// </summary>
        /// <param name="logDate">Format: dd-MM-yyyy</param>
        /// <param name="logLevel">Log level</param>
        /// <param name="contains">Logs assigned to classes, which names contains this string will be returend.</param>
        /// <param name="limit">Limit returned logs. (order by latest)</param>
        /// <returns>
        ///     Returns list of logs, or empty list if no logs were found.
        /// </returns>
        /// <response code="200">When returned at least one log.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLogsFromDB([FromQuery]string? logDate, [FromQuery]LogLevel? logLevel, [FromQuery]string? contains, [FromQuery]int amount, [FromQuery]int start )
        {
            object[] result = await Mediator.Send(new GetLogsFromDBQuery { LogDate = logDate, LogLevel = logLevel, NameContains = contains, Amount = amount, StartPosition = start });
            HttpContext.InsertPaginationParameterInResponseHeader((int)result[0], amount);
            return Ok((List<LogEntryVO>)result[1]);
        }

        /// <summary>
        ///     Deletes selected application logs from database.
        /// </summary>
        /// <param name="logDate">Format: dd-MM-yyyy</param>
        /// <param name="logLevel">Log level</param>
        /// <param name="contains">Logs assigned to classes, which names contains this string will be removed.</param>
        /// <returns>
        ///     Removes list of found logs with given conditions, or does nothing if no logs were found.
        /// </returns>
        /// <response code="200">When removed at least one log.</response>
        /// <response code="404">When no logs were found.</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearLogs(string logDate, LogLevel logLevel, string contains)
        {
            return Ok(await Mediator.Send(new ClearLogsFromFileSystemCommand { LogDate = logDate, LogLevel = logLevel, NameContains = contains }));
        }
    }
}