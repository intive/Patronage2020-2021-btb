using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Models.Validation
{
    public class AlertFormModelValidator : AbstractValidator<AlertFormModel>
    {
        public AlertFormModelValidator()
        {
            RuleFor(i => i.SymbolPair)
                .MaximumLength(20).WithMessage("Symbol pair cannot be longer than 20 characters.")
                .Matches("^$|^([A-Z]{5,20})$").WithMessage("Trading pair format is incorrect.")
                .NotEmpty();

            RuleFor(a => a.Condition)
                .NotEmpty().WithMessage("Please choose a condition.");

            RuleFor(a => a.ValueType)
                .NotEmpty().WithMessage("Please choose a value type.");

            RuleFor(a => a.Value)
                .GreaterThan(0.0d).WithMessage("Enter a number greater than zero.");

            RuleFor(a => a.Email)
                .NotEmpty().When(a => a.SendEmail).WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(a => a.Message)
                .NotNull().When(a => a.SendEmail).WithMessage("Email message is required.")
                .MaximumLength(500).WithMessage("Maximum message length is 500.");
        }
    }
}
