using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTB.Application.Example.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    public class ExampleController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> test()
        {
            return Ok(await Mediator.Send(new ExampleQuery()));
        }
    }
}