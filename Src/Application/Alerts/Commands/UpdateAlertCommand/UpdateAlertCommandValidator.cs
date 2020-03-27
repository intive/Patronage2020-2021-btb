﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Alerts.Commands.UpdateAlertCommand
{
    class UpdateAlertCommandValidator : AbstractValidator<UpdateAlertCommand>
    {
        public UpdateAlertCommandValidator()
        {
            RuleFor(a => a.Symbol)
                .MaximumLength(10)
                .Matches("^$|^([A-Z]{5,10})$")
                .NotEmpty();

            RuleFor(a => a.Condition)
                .NotEmpty();

            RuleFor(a => a.ValueType)
                .NotEmpty();

            RuleFor(a => a.Value)
                .NotEmpty()
                .GreaterThan(0);

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