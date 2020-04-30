using FluentValidation.Results;
using FluentValidation;

namespace BTB.Application.UserManagement.Commands.TakeRoleFromUser
{
    class TakeRoleFromUserCommandValidator : AbstractValidator<TakeRoleFromUserCommand>
    {
        public TakeRoleFromUserCommandValidator()
        {
            RuleFor(i => i.Role)
                .NotEmpty();

            RuleFor(i => i.UserName)
                .NotEmpty();
        }

        protected override bool PreValidate(ValidationContext<TakeRoleFromUserCommand> context, ValidationResult result)
        {
            if (context.InstanceToValidate.Role == null && context.InstanceToValidate.UserName == null)
            {
                result.Errors.Add(new ValidationFailure("Syntax Error", "Please ensure the request body is properly formatted JSON."));
                return false;
            }
            return true;
        }
    }
}
