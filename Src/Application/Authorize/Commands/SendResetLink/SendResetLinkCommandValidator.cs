using FluentValidation;

namespace BTB.Application.Authorize.Commands.SendResetLink
{
    public class SendResetLinkCommandValidator : AbstractValidator<SendResetLinkCommand>
    {
        public SendResetLinkCommandValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
