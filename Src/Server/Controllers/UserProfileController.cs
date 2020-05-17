using System.Threading;
using System.Threading.Tasks;
using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using BTB.Application.UserProfile.Common;
using BTB.Application.UserProfile.Queries.GetUserProfileQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class UserProfileController : BaseController
    {
        /// <summary>
        /// Returns user profile information.
        /// </summary>
        /// <returns> A DTO containing user profile information. <see cref="UserProfileInfoVm"/></returns>
        /// <response code="200">When successful.</response>
        /// <response code="404">If a user with given id does not exist or has not created a profile yet.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetUserProfileQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Updates user profile information.
        /// </summary>
        /// <param name="command"> From-body data to update the profile.</param>
        /// <response code="200">If user profile is updated successfully.</response>
        /// <response code="400">If a validation error occurs.</response>
        /// <response code="404">If a user with given id does not exist or has not created a profile yet.</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}