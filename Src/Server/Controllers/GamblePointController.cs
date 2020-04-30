using System.Threading.Tasks;
using BTB.Application.GamblePoints.Queries.GetGamblePoints;
using BTB.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BTB.Server.Controllers
{
    public class GamblePointController : BaseController
    {
        /// <summary>
        /// Returns current user gamble points vo.
        /// </summary>
        /// <returns>User gamble points vo.<see cref="GamblePointsVO"/></returns>
        /// <response code="200">When successful.</response>
        /// <response code="400">When can't find user points.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPoints()
        {
            GamblePointsVO userFreePoints = await Mediator.Send(new GetGamblePointsQuery());
            return Ok(userFreePoints);
        }
    }
}