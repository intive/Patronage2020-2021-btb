using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.GamblePoints.Queries.GetGamblePoints
{
    public class GetGamblePointsQuery : IRequest<GamblePointsVO>
    {
    }
}
