using BTB.Application.Authorize.Commands.ChangePassword;
using BTB.Application.Authorize.Commands.Login;
using BTB.Application.Authorize.Commands.Logout;
using BTB.Application.Authorize.Commands.Register;
using BTB.Application.Authorize.Commands.ResetPassword;
using BTB.Application.Authorize.Commands.SendResetLink;
using BTB.Application.Authorize.Queries.GetUserInfo;
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
        /// <returns>A view model containing information about current user's credentials. <see cref="UserInfoVm"/></returns>
        /// <response code="200">When succesful.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoVm>> UserInfo()
        {
            var userInfo = await Mediator.Send(new GetUserInfoQuery { User = User });
            return Ok(userInfo);
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

        /// <summary>
        /// Changes the password of the currently logged in user.
        /// </summary>
        /// <param name="changePasswordCommand"> From-body data to update password.</param>
        /// <response code="200">When successful.</response>
        /// <response code="404">When unable to find user or password change failed.</response>
        [Authorize]
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand)
        {
            var result = await Mediator.Send(resetPasswordCommand);
            return Ok(result);
        }
    }
}