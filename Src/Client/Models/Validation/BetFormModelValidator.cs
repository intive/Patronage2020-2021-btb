using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Models.Validation
{
    public class BetFormModelValidator : AbstractValidator<BetFormModel>
    {
        public BetFormModelValidator()
        {
            var greaterThanZeroMessage = "Please enter a number greater than zero.";
            var maxValueMessage = "This value cannot exceed 999,999,999.999999999.";

            RuleFor(b => b.SymbolPair)
                .MaximumLength(20).WithMessage("Symbol pair cannot be longer than 20 characters.")
                .Matches("^$|^([A-Z]{5,20})$").WithMessage("Trading pair format is incorrect.")
                .NotEmpty();

            RuleFor(b => b.Points)
                .GreaterThan(0.0m).WithMessage(greaterThanZeroMessage)
                .LessThan(999999999.999999999m).WithMessage(maxValueMessage);

            RuleFor(b => b.LowerPriceThreshold)
                .GreaterThan(0.0m).WithMessage(greaterThanZeroMessage)
                .LessThan(999999999.999999999m).WithMessage(maxValueMessage);

            RuleFor(b => b.UpperPriceThreshold)
                .GreaterThan(b => b.LowerPriceThreshold).WithMessage("Must be greater than the first value.")
                .LessThan(999999999.999999999m).WithMessage(maxValueMessage);

            var validTypes = new string[] { "standard" };
            RuleFor(b => b.RateType)
                .NotEmpty()
                .Must(type => validTypes.Contains(type.ToLower())).WithMessage("Please choose a bet type.");

            var validIntervals = new string[] { "twodays" };
            RuleFor(b => b.TimeInterval)
                .NotEmpty()
                .Must(interval => validIntervals.Contains(interval.ToLower())).WithMessage("Please select bet duration.");
        }
    }
}
