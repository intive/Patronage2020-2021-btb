using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net.Objects;
using BTB.Application.Common.Models;
using BTB.Application.System.Commands.AddKlineCommand;
using BTB.Application.System.Commands.ClearData;
using BTB.Application.System.Commands.LoadData;
using BTB.Application.System.Commands.SendEmailCommand;
using BTB.Application.System.Commands.SendEmailNotificationsCommand;
using BTB.Application.System.Queries.GetAuditTrail;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class SystemController : BaseController
    {

        /// <summary>
        ///     Loads symbols and symbol-pairs from database, if not loaded yet
        /// </summary>
        /// <returns>
        ///     No return data.
        /// </returns>
        /// <response code="200">When successful.</response>
        /// <response code="503">If there was a problem with loading data from Binance.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> LoadSymbolsAndPairs()
        {
            return Ok(await Mediator.Send(new LoadSymbolsCommand(), CancellationToken.None));
        }

        /// <summary>
        ///     Loads given amount of klines to DB
        /// </summary>
        /// <param name="klineType">
        ///     Type of kline to be loaded
        /// </param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> LoadKlines(TimestampInterval klineType, int amount)
        {
            return Ok(await Mediator.Send(new LoadKlinesCommand() { KlineType = klineType, Amount = amount }, CancellationToken.None));
        }

        /// <summary>
        ///     Deletes selected Klines from database, which has OpenTime between given timestamp.    
        /// </summary>
        /// <param name="klineType">
        ///     Kline interval. Pass 'Zero' to affect all types of klines.
        /// </param>
        /// <param name="startTimestamp">
        ///     Start date in seconds-timestamp
        /// </param>
        /// <param name="stopTimestamp">
        ///     Stop date in seconds-timestamp.
        /// </param>
        /// <returns>
        ///     0 on success
        ///     -1 when given type of kline is not stored in Database
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> ClearKlines(long startTimestamp, long stopTimestamp, TimestampInterval klineType)
        {
            return Ok(await Mediator.Send(new ClearKlinesCommand() { StartTime = DateTimestampConv.GetDateTime(startTimestamp), StopTime = DateTimestampConv.GetDateTime(stopTimestamp), KlineType = klineType  }, CancellationToken.None));
        }

        /// <summary>
        /// Get last N Audits
        /// </summary>
        /// <param name="count"> maximum number of audits to return </param>
        /// <response code="200">If successful.</response>
        /// <response code="500">If error occur during request.</response>
        /// <returns> List of <see cref="AuditTrail"/> </returns>
        [Route("audits")]
        [HttpGet]
        public async Task<IActionResult> GetAudits(int count)
        {
            return Ok(await Mediator.Send(new GetAuditsQuery { Count = count }, CancellationToken.None));
        }

        /// <summary>
        /// Sends a test email message. If the recipient's address is not provided, then
        /// the message will be sent to "patronagebtb@gmail.com".
        /// </summary>
        /// <param name="command">An object containing email data.</param>
        /// <response code="200">When successful.</response>
        [Route("email")]
        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// For testing purposes only!
        /// Creates a dummy kline in the database for a specific trading pair.
        /// This endpoint also triggers the email alert to make testing easy.
        /// </summary>
        /// <param name="command">An object containing kline data.</param>
        /// <response code="200">When successful.</response>
        [Route("kline")]
        [HttpPost]
        public async Task<IActionResult> AddKline(AddKlineCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);
            await Mediator.Send(new SendNotificationsCommand() { KlineInterval = TimestampInterval.FiveMin }, cancellationToken);
            return Ok();
        }
    }
}