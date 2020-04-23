using BTB.Client.Models.Authorization;
using FluentValidation;

namespace BTB.Client.Models.Validation
{
    public class RegisterParametersModelValidator : AbstractValidator<RegisterParametersModel>
    {
        public RegisterParametersModelValidator()
        {
            RuleFor(a => a.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(4).WithMessage("Username must be at least 4 characters long");

            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(4).WithMessage("Password must be at least 4 characters long ");

            RuleFor(a => a.PasswordConfirm)
                .NotEmpty().WithMessage("Password confirmation is required")
                .Matches(a => a.Password).WithMessage("Passwords do not match");
        }
    }
}
