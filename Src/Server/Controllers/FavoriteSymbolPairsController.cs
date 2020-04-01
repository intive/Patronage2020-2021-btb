using BTB.Application.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair;
using BTB.Application.FavoriteSymbolPairs.Commands.DeleteFavoriteSymbolPair;
using BTB.Application.FavoriteSymbolPairs.Queries.GetAllFavoriteSymbolPairs;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using BTB.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Controllers
{
    [Authorize]
    public class FavoriteSymbolPairsController : BaseController
    {
        /// <summary>
        /// Returns all favorite symbol pairs belonging to the currently logged in user.
        /// </summary>
        /// <returns>A paginated list of favorite symbol pairs DTOs. <see cref="SymbolPairVO" /></returns>
        /// <response code="200">When successful.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination, CancellationToken cancellationToken)
        {
            var paginatedResult = await Mediator.Send(new GetAllFavoriteSymbolPairsQuery() { Pagination = pagination }, cancellationToken);
            HttpContext.InsertPaginationParameterInResponseHeader(paginatedResult.AllRecorsCount, paginatedResult.RecordsPerPage);
            return Ok(paginatedResult.Result);
        }

        /// <summary>
        /// Creates an favorite symbol pair for the currently logged in user.
        /// </summary>
        /// <param name="id">Id of the symbol pair.</param>
        /// <returns>An object containing the data that was created.</returns>
        /// <response code="201">When favorite symbol pair is created successfully.</response>
        /// <response code="400">When validation error occurs.</response>
        [Route("{id}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
        {
            return CreatedAtAction(nameof(Create), await Mediator.Send(new CreateFavoriteSymbolPairCommand() { SymbolPairId = id }, cancellationToken));
        }

        /// <summary>
        /// Deletes an favorite symbol pair specified by id. The favorite symbol pair must belong to the logged in user.
        /// </summary>
        /// <param name="id">Id of the favorite symbol pair.</param>
        /// <response code="200">When successful.</response>
        /// <response code="404">When alert with given id does not exist or does not belong to the user.</response>
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteFavoriteSymbolPairCommand() { SymbolPairId = id }, cancellationToken);
            return Ok();
        }
    }
}