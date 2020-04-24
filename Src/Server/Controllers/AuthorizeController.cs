using BTB.Application.Authorize.Commands.ChangePassword;
using BTB.Application.Authorize.Commands.Login;
using BTB.Application.Authorize.Commands.Register;
using BTB.Application.Authorize.Commands.ResetPassword;
using BTB.Application.Authorize.Commands.SendResetLink;
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Registers the user. Username and password has to be at least 4 characters long.
        /// </summary>
        /// <returns>Request's result.</returns>
        /// <response code="200">If username, password and email requirement were met and when password confirm is the same as the password.</response>
        /// <response code="400">If username, password and email requirement were NOT met or when password confirm is different than password.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Changes the password of the currently logged in user.
        /// </summary>
        /// <param name="changePasswordCommand"> From-body data to update password.</param>
        /// <response code="200">When successful.</response>
        /// <response code="404">When unable to find user or password change failed.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
        {
            var result = await Mediator.Send(changePasswordCommand);
            return Ok(result);
        }

        /// <summary>
        /// Send reset link on user email for reseting password.
        /// </summary>
        /// <param name="sendResetLinkCommand"> From-body data to send reset link.</param>
        /// <response code="200">It's always 200 for security reasons.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> SendResetLink([FromBody] SendResetLinkCommand sendResetLinkCommand)
        {
            var result = await Mediator.Send(sendResetLinkCommand);
            return Ok(result);
        }

        /// <summary>
        /// Reset password based on token and email in query.
        /// </summary>
        /// <param name="resetPasswordCommand"> From-body data to reset password. Includes token and email.</param>
        /// <response code="200">When successful.</response>
        /// /// <response code="404">When unable to reset password.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand)
        {
            var result = await Mediator.Send(resetPasswordCommand);
            return Ok(result);
        }
    }
}