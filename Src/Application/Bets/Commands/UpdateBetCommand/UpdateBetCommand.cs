using BTB.Application.Bets.Common;
using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.Bets.Commands.UpdateBetCommand
{
    public class UpdateBetCommand : BetRequestBase, IRequest<BetVO>
    {
        public int Id { get; set; }
    }
}
