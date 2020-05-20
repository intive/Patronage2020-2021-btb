using BTB.Client.Models.Authorization;
using FluentValidation;

namespace BTB.Client.Models.Validation
{
    public class RegisterParametersModelValidator : AbstractValidator<RegisterParametersModel>
    {
        public RegisterParametersModelValidator()
        {
            RuleFor(a => a.UserName)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(a => a.UserName)
                .MinimumLength(4).WithMessage("Username must be at least 4 characters long")
                .When(a => !string.IsNullOrEmpty(a.UserName));

            RuleFor(a => a.DisplayName)
                .NotEmpty().WithMessage("Display name is required");

            RuleFor(a => a.DisplayName)
                .MinimumLength(5).WithMessage("Display name must be at least 5 characters long")
                .MaximumLength(16).WithMessage("Display name can't be longer than 16 characters")
                .When(a => !string.IsNullOrEmpty(a.DisplayName));

            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email is required");

            RuleFor(a => a.Email)
                .EmailAddress().WithMessage("Email is not correct")
                .When(a => !string.IsNullOrEmpty(a.Email));

            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Password is required");

            RuleFor(a => a.Password)
                .MinimumLength(4).WithMessage("Password must be at least 4 characters long ")
                .When(a => !string.IsNullOrEmpty(a.Password));

            RuleFor(a => a.PasswordConfirm)
                .NotEmpty().WithMessage("Password confirmation is required");

            RuleFor(a => a.PasswordConfirm)
                .Matches(a => a.Password).WithMessage("Passwords do not match")
                .When(a => !string.IsNullOrEmpty(a.PasswordConfirm));
        }
    }
}