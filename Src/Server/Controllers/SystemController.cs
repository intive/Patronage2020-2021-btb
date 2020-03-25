using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net.Objects;
using BTB.Application.Common.Models;
using BTB.Application.System.Commands.ClearData;
using BTB.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class SystemController : BaseController
    {
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
    }
}