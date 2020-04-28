using MediatR;

namespace BTB.Application.GamblePoints.Commands.AddValueToAllGamblePoints
{
    public class AddValueToAllGamblePointsCommand : IRequest
    {
        public decimal Amount { get; set; }
    }
}
