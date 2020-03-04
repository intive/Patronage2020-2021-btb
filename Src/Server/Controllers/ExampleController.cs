using BTB.Application.Example.Queries.GetExampleItem;
using BTB.Application.Example.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BTB.Application.Common.Interfaces;

namespace BTB.Server.Controllers
{
    public class ExampleController : BaseController
    {
        public ExampleController(IEmailService emailService)
        {
            emailService.Send("receiver@gmail.com", "title", "message");
        }

        [HttpGet]
        public async Task<IActionResult> test()
        {
            return Ok(await Mediator.Send(new ExampleQuery()));
        }

        /* Added for the purpose of testing global exception handling,
         * In case you're using localhost: https://localhost:[YOUR_PORT_NUMBER]/api/example/1
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetItem(int id)
        {
            return await Mediator.Send(new GetExampleItemQuery { Id = id });
        }
    }
}