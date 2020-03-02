using BTB.Application.Example.Queries.GetExampleItem;
using BTB.Application.Example.Queries.ExampleQuery;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTB.Server.Controllers
{
    public class ExampleController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> test()
        {
            return Ok(await Mediator.Send(new ExampleQuery()));
        }

        /* Added for the purpose of testing global exception handling,
         * How to use it: https://localhost:[YOUR_PORT_NUMBER]/api/example/1
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetItem(int id)
        {
            return await Mediator.Send(new GetExampleItemQuery { Id = id });
        }
    }
}