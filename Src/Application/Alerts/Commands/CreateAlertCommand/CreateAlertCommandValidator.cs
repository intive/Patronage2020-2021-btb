using FluentValidation;

namespace BTB.Application.Alerts.Commands.CreateAlertCommand
{
    class CreateAlertCommandValidator : AbstractValidator<CreateAlertCommand>
    {
        public CreateAlertCommandValidator()
        {
            RuleFor(a => a.SymbolPair)
                .MaximumLength(20)
                .Matches("^$|^([A-Z]{5,20})$")
                .NotEmpty();

            RuleFor(a => a.Condition)
                .NotEmpty();

            RuleFor(a => a.ValueType)
                .NotEmpty();

            RuleFor(a => a.Value)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(a => a.SendEmail)
                .NotNull();

            RuleFor(a => a.Email)
                .NotEmpty().When(a => a.SendEmail)
                .EmailAddress();

            RuleFor(a => a.Message)
                .NotNull().When(a => a.SendEmail)
                .MaximumLength(500);
        }
    }
}
