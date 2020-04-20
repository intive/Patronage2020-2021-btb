using FluentValidation;
using FluentValidation.Results;


namespace BTB.Application.Indicator.Commands.CalculateSMA
{
    public class CalculateSMACommandValidator : AbstractValidator<CalculateSMACommand>
    {
        public CalculateSMACommandValidator()
        {
            RuleFor(i => i.Prices)
                .NotNull();

            RuleForEach(i => i.Prices)
                .NotNull()
                .Must(s => decimal.TryParse(s, out decimal result))
                .WithMessage("Only numbers are allowed.");

            RuleFor(i => i.TimePeriod)
                .NotNull()
                .GreaterThan(1);
        }

        protected override bool PreValidate(ValidationContext<CalculateSMACommand> context, ValidationResult result)
        {
            if (context.InstanceToValidate.Prices == null
                && context.InstanceToValidate.TimePeriod == null)
            {
                result.Errors.Add(new ValidationFailure("Syntax Error", "Please ensure the request body is properly formatted JSON."));
                return false;
            }
            return true;
        }
    }

    
}
