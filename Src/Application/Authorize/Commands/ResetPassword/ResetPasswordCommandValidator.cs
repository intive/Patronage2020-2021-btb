using FluentValidation;

namespace BTB.Application.Authorize.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(c => c.Password)
                 .NotNull()
                 .MinimumLength(4);

            RuleFor(c => c.ConfirmPassword)
                .Equal(c => c.Password)
                .WithMessage("Passwords should match.");
        }
    }
}
