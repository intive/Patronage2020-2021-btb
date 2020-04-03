using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.UserProfile.Common
{
    public class UserProfileInfoRequestValidator : AbstractValidator<UserProfileInfoRequestBase>
    {
        public UserProfileInfoRequestValidator()
        {
            RuleFor(command => command.Username)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(16);

            RuleFor(command => command.ProfileBio)
                .MaximumLength(256);

            RuleFor(command => command.FavouriteTradingPair)
                .MaximumLength(20)
                .Matches("^$|^([A-Z]{5,20})$");
        }
    }
}
