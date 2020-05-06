using BTB.Application.Bets.Common;
using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.Bets.Commands.CreateBetCommand
{
    public class CreateBetCommand : BetRequestBase, IRequest<BetVO>
    {

    }
}
