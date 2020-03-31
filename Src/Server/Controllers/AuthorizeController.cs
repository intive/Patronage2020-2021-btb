using BTB.Application.Authorize.Commands.Login;
using BTB.Application.Authorize.Commands.Logout;
using BTB.Application.Authorize.Commands.Register;
using BTB.Application.Authorize.Common;
using BTB.Application.Authorize.Queries.GetAuthorizationInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BTB.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : BaseController
    {
        /// <summary>
        /// Login the user.
        /// </summary>
        /// <returns>Request's result.</returns>
        /// <response code="200">When username and password is correct and login was successful.</response>
        /// <response code="400">When username or password was incorrect.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Returns information about current user's credentials.
        /// </summary>
        /// <returns>A data transfer object containing information about current user's credentials. <see cref="AuthorizationInfoDto"/></returns>
        /// <response code="200">When succesful.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthorizationInfoDto>> GetAuthorizationInfo()
        {
            var authInfo = await Mediator.Send(new GetAuthorizationInfoQuery { User = User });
            return Ok(authInfo);
        }

        /// <summary>
        /// Registers the user. Username and password has to be at least 4 characters long.
        /// </summary>
        /// <returns>Request's result.</returns>
        /// <response code="200">If username and password requirement were met and when password confirm is the same as the password.</response>
        /// <response code="400">If username and password requirement were NOT met or when password confirm is different than password.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Logs out currently logged in user.
        /// </summary>
        /// <returns>Request's result.</returns>
        /// <response code="200">If user logs out.</response>
        /// <response code="401">If no user is currently logged in.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            var result = await Mediator.Send(new LogoutCommand());
            return Ok(result);
        }
    }
}