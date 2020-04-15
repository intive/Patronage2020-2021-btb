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

            RuleFor(command => command.OpenPrice)
                .NotEmpty()
                .GreaterThan(0.0m)
                .LessThan(999999999.999999999m);

            RuleFor(command => command.ClosePrice)
                .NotEmpty()
                .GreaterThan(0.0m)
                .LessThan(999999999.999999999m);

            RuleFor(command => command.Volume)
                .NotEmpty()
                .GreaterThanOrEqualTo(0.0m)
                .LessThan(999999999.999999999m);
        }
    }
}
