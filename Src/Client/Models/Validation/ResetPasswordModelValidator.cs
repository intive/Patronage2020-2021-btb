using FluentValidation;

namespace BTB.Client.Models.Validation
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
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
