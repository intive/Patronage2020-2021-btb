using BTB.Application.Example.Queries.GetExampleItem;
using BTB.Application.Example.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BTB.Application.Common.Interfaces;

namespace BTB.Server.Controllers
{
    /// <summary>
    ///     Example controller class
    /// </summary>
    public class ExampleController : BaseController
    {
        private readonly IEmailService _emailServiceTest;

        public ExampleController(IEmailService emailService)
        {
            this._emailServiceTest = emailService;
        }

        /// <summary>
        ///     Tries to send email to given adress.
        ///     Please remember to enter your e-mail credentials to appsettings.json.
        /// </summary>
        /// <param name="emailAdress">
        ///     Email adress to which mail should be sent.
        /// </param>
        /// <returns>
        ///     Ok() on success, throws exception on fail.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> emailTest(string emailAdress)
        {
            _emailServiceTest.Send(emailAdress, "title", "message");
            return Ok("Email Sent");
        }

        /// <summary>
        ///     Test method description.
        /// </summary>
        /// <returns>
        ///     Test return value.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> test()
        {
            return Ok(await Mediator.Send(new ExampleQuery()));
        }

        /// <summary>
        ///     Test global exception handling method.
        /// </summary>
        /// <param name="id">
        ///     Test item id.
        /// </param>
        /// <returns>
        ///     Throws custom "NotFoundException". 
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetItem(int id)
        {
            return await Mediator.Send(new GetExampleItemQuery { Id = id });
        }
    }
}