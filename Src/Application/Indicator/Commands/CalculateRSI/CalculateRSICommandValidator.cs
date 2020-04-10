using FluentValidation;
using FluentValidation.Results;
using BTB.Domain.Common.Indicator;

namespace BTB.Application.Indicator.Commands.CalculateRSI
{
    public class CalculateRSICommandValidator : AbstractValidator<CalculateRSICommand>
    {
        public CalculateRSICommandValidator()
        {
            RuleFor(i => i.ClosePrices)
                .NotNull();

            RuleForEach(i => i.ClosePrices)
                .NotNull()
                .Must(s => decimal.TryParse(s, out decimal result))
                .WithMessage("Only numbers are allowed.");

            RuleFor(e => e.Timeframe)
                .NotNull()
                .IsInEnum();
            RuleFor(e => e.Timeframe.Value.ToString())
                .IsEnumName(typeof(RSITimeframe), caseSensitive: false);
        }

        protected override bool PreValidate(ValidationContext<CalculateRSICommand> context, ValidationResult result)
        {
            if (context.InstanceToValidate.ClosePrices == null
                && context.InstanceToValidate.Timeframe == null)
            {
                result.Errors.Add(new ValidationFailure("Syntax Error", "Please ensure the request body is properly formatted JSON."));
                return false;
            }
            return true;
        }

    }
}
