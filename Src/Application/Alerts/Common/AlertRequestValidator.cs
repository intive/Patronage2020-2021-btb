using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTB.Application.Alerts.Common
{
    public class AlertRequestValidator : AbstractValidator<AlertRequestBase>
    {
        public AlertRequestValidator()
        {
            RuleFor(a => a.SymbolPair)
                .MaximumLength(20)
                .Matches("^$|^([A-Z]{5,20})$")
                .NotEmpty();

            var validConditions = new string[] { "crossing" };
            RuleFor(a => a.Condition)
                .NotEmpty()
                .Must(condition => validConditions.Contains(condition.ToLower()));

            var validValueTypes = new string[] { "price", "volume" };
            RuleFor(a => a.ValueType)
                .NotEmpty()
                .Must(valueType => validValueTypes.Contains(valueType.ToLower()));

            RuleFor(a => a.Value)
                .NotNull()
                .GreaterThan(0.0m)
                .LessThan(999999999.999999999m);

            RuleFor(a => a.SendEmail)
                .NotNull();

            RuleFor(a => a.Email)
                .NotEmpty().When(a => a.SendEmail)
                .EmailAddress();

            RuleFor(a => a.Message)
                .NotNull().When(a => a.SendEmail)
                .MaximumLength(500);
        }
    }
}
