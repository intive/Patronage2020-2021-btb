using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Commands.SendEmailCommand
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(command => command.To)
                .NotEmpty()
                .EmailAddress();

            RuleFor(command => command.Title)
                .NotEmpty();

            RuleFor(command => command.Content)
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
