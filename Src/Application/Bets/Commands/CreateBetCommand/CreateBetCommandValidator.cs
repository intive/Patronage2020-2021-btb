using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTB.Application.Bets.Commands.CreateBetCommand
{
    public class CreateBetCommandValidator : AbstractValidator<CreateBetCommand>
    {
        public CreateBetCommandValidator()
        {
            RuleFor(b => b.SymbolPair)
                .MaximumLength(20)
                .Matches("^$|^([A-Z]{5,20})$")
                .NotEmpty();

            RuleFor(b => b.Points)
                .GreaterThan(0.0m);

            var validTypes = new string[] { "standard" };
            RuleFor(b => b.RateType)
                .NotEmpty()
                .Must(type => validTypes.Contains(type.ToLower()));

            var validIntervals = new string[] { "twodays" };
            RuleFor(b => b.TimeInterval)
                .NotEmpty()
                .Must(interval => validIntervals.Contains(interval.ToLower()));
        }
    }
}
