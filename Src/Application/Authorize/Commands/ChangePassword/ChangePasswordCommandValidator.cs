using FluentValidation;

namespace BTB.Application.Authorize.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(c => c.CurrentPassword)
                .NotNull();

            RuleFor(c => c.NewPassword)
                .MinimumLength(4);

            RuleFor(c => c.NewPasswordConfirmation)
                .Equal(c => c.NewPassword)
                .WithMessage("Passwords should match.");
        }
       
    }
}
