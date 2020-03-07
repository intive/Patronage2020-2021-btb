using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTB.Application.Dashboard.Queries.GetTopListQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BTB.Server.Controllers
{
    public class DashboardController : BaseController
    {
        [Route("top")]
        [HttpGet]
        public async Task<IActionResult> GetTop()
        {
            return Ok(await Mediator.Send(new GetTopListQuery()));
        }
    }
}
