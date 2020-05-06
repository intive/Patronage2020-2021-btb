using MediatR;

namespace BTB.Application.System.Commands.AddValueToAllGamblePoints
{
    public class AddValueToAllGamblePointsCommand : IRequest
    {
        public decimal Amount { get; set; }
    }
}
