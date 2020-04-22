using FluentValidation;

namespace BTB.Client.Models.Validation
{
    public class ForgotPasswordModelValidator : AbstractValidator<ForgotPasswordModel>
    {
        public ForgotPasswordModelValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
