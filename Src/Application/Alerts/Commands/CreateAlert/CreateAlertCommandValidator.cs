using FluentValidation;

namespace BTB.Application.Alerts.Commands.CreateAlert
{
    class CreateAlertCommandValidator : AbstractValidator<CreateAlertCommand>
    {
        public CreateAlertCommandValidator()
        {
            RuleFor(a => a.Symbol)
                .NotEmpty().WithMessage("Symbol is empty.");

            RuleFor(a => a.Condition)
                .NotEmpty().WithMessage("Condition is empty.");

            RuleFor(a => a.ValueType)
                .NotEmpty().WithMessage("ValueType is empty.");

            RuleFor(a => a.Value)
                .NotEmpty().WithMessage("Value is empty.")
                .GreaterThan(0).WithMessage("Value must be greather than 0.");

            RuleFor(a => a.SendEmail)
                .NotNull().WithMessage("Send email field is empty.");

            RuleFor(a => a.Email)
                .NotEmpty()
                .WithMessage("Email is empty.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(a => a.Message)
                .MaximumLength(500).WithMessage("Message is to long.");
        }
    }
}
