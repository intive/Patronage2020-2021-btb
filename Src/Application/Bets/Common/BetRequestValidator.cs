using FluentValidation;
using System.Linq;

namespace BTB.Application.Bets.Common
{
    public class BetRequestValidator : AbstractValidator<BetRequestBase>
    {
        public BetRequestValidator()
        {
            RuleFor(b => b.SymbolPair)
                .MaximumLength(20)
                .Matches("^$|^([A-Z]{5,20})$")
                .NotEmpty();

            RuleFor(b => b.Points)
                .NotNull()
                .GreaterThan(0.0m)
                .LessThan(999999999.999999999m);

            RuleFor(b => b.LowerPriceThreshold)
                .NotNull()
                .GreaterThan(0.0m)
                .LessThan(999999999.999999999m);

            RuleFor(b => b.UpperPriceThreshold)
                .NotNull()
                .GreaterThan(b => b.LowerPriceThreshold)
                .LessThan(999999999.999999999m);

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
