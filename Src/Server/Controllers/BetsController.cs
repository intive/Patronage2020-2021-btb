using BTB.Application.Bets.Commands.CreateBetCommand;
using BTB.Application.Bets.Commands.DeleteBetCommand;
using BTB.Application.Bets.Commands.UpdateBetCommand;
using BTB.Application.Bets.Queries.GetAllActiveBetsQuery;
using BTB.Domain.Common.Pagination;
using BTB.Domain.Extensions;
using BTB.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class BetsController : BaseController
    {
        /// <summary>
        /// Returns all active bets.
        /// </summary>
        /// <param name="pagination">Pagination DTO.</param>
        /// <returns>A paginated list of bet objects. <see cref="BetVO"/></returns>
        /// <response code="200">When successful.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActivbeBets([FromQuery] PaginationDto pagination, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid pagination data." });
            }

            var paginatedResult = await Mediator.Send(new GetAllActiveBetsQuery() { Pagination = pagination }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(paginatedResult.AllRecordsCount, paginatedResult.RecordsPerPage);
            return Ok(paginatedResult.Result);
        }

        /// <summary>
        /// Creates a bet for the logged in user.
        /// </summary>
        /// <param name="command"> From-body data to create a bet.</param>
        /// <returns>The object that was created.</returns>
        /// <response code="201">When the bet is created successfully.</response>
        /// <response code="400">When a validation error occurs.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBet([FromBody] CreateBetCommand command, CancellationToken cancellationToken)
        {
            return CreatedAtAction(nameof(CreateBet), await Mediator.Send(command, cancellationToken));
        }

        /// <summary>
        /// Updates bet specified by id. The bet must belong to the logged in user.
        /// </summary>
        /// <param name="id">Id of the bet.</param>
        /// <param name="command"> From-body data to update bet.</param>
        /// <response code="200">When successful.</response>
        /// <response code="400">When a validation error occurs.</response>
        /// <response code="404">When bet with given id does not exist.</response>
        [Route("id")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBet(int id, [FromBody] UpdateBetCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return BadRequest();
            }

            command.Id = id;
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Deletes bet specified by id. The bet must belong to the logged in user.
        /// </summary>
        /// <param name="id">Id of the bet.</param>
        /// <response code="200">When successful.</response>
        /// <response code="404">When bet with given id does not exist or does not belong to the user.</response>
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteBetCommand { Id = id }, cancellationToken);
            return Ok();
        }
    }
}
