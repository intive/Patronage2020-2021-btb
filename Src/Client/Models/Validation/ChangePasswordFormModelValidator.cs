using FluentValidation;
namespace BTB.Client.Models.Validation
{
    public class ChangePasswordFormModelValidator : AbstractValidator<ChangePasswordFormModel>
    {
        public ChangePasswordFormModelValidator()
        {
            RuleFor(c => c.CurrentPassword)
                .NotNull();

            RuleFor(c => c.NewPassword)
                .NotNull()
                .MinimumLength(4);

            RuleFor(c => c.NewPasswordConfirmation)
                .NotNull()
                .Equal(c => c.NewPassword)
                .WithMessage("Passwords should match.");
        }   
    }
}
