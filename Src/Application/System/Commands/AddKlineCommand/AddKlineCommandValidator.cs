using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Commands.AddKlineCommand
{
    public class AddKlineCommandValidator : AbstractValidator<AddKlineCommand>
    {
        public AddKlineCommandValidator()
        {
            RuleFor(command => command.SymbolPair)
                .MaximumLength(20)
                .Matches("^$|^([A-Z]{5,20})$")
                .NotEmpty();

            RuleFor(command => command.ClosePrice)
                .NotEmpty()
                .GreaterThan(0.0m);

            RuleFor(command => command.Volume)
                .NotEmpty()
                .GreaterThan(0.0m);
        }
    }
}
