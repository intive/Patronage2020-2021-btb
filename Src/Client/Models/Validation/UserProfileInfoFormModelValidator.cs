using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Models.Validation
{
    public class UserProfileInfoFormModelValidator : AbstractValidator<UserProfileInfoFormModel>
    {
        public UserProfileInfoFormModelValidator()
        {
            RuleFor(i => i.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(5).WithMessage("Username must be at least 5 characters long.")
                .MaximumLength(16).WithMessage("Username cannot be longer than 16 characters.");

            RuleFor(i => i.ProfileBio)
                .MaximumLength(256).WithMessage("Profile bio cannot be longer than 256 characters.");

            RuleFor(i => i.FavouriteTradingPair)
                .MaximumLength(10).WithMessage("Trading pair symbol cannot be longer than 10 characters.")
                .Matches("^$|^([A-Z]{5,10})$").WithMessage("Trading pair format is incorrect.");
        }
    }
}
