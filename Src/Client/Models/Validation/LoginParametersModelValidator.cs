using BTB.Client.Models.Authorization;
using FluentValidation;

namespace BTB.Client.Models.Validation
{
    public class LoginParametersModelValidator : AbstractValidator<LoginParametersModel>
    {
        public LoginParametersModelValidator()
        {
            RuleFor(a => a.UserName)
                .NotEmpty().WithMessage("Please enter a username");

            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Please enter a password");
        }
    }
}
