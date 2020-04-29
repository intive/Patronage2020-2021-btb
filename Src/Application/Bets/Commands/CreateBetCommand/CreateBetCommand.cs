using BTB.Domain.Enums;
using BTB.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Bets.Commands.CreateBetCommand
{
    public class CreateBetCommand : IRequest<BetVO>
    {
        public string SymbolPair { get; set; }
        public decimal Points { get; set; }
        public string RateType { get; set; }
        public string TimeInterval { get; set; }
    }
}
