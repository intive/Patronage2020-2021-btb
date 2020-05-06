using MediatR;

namespace BTB.Application.Bets.Commands.DeleteBetCommand
{
    public class DeleteBetCommand : IRequest
    {
        public int Id { get; set; }
    }
}
