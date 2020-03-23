using FluentValidation;

namespace BTB.Application.Authorize.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(command => command.UserName)
                .MinimumLength(4);

            RuleFor(command => command.Password)
                .MinimumLength(4);

            RuleFor(command => command.PasswordConfirm)
                .Equal(command => command.Password)
                .WithMessage("Passwords should match.");
        }
    }
}