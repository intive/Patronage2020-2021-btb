using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.UserProfile.Commands.CreateUserProfileCommand
{
    class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
    {
        public CreateUserProfileCommandValidator()
        {
            RuleFor(command => command.Username)
                .NotEmpty()
                .MaximumLength(16);

            RuleFor(command => command.ProfileBio)
                .MaximumLength(256);

            RuleFor(command => command.FavouriteTradingPair)
                .MaximumLength(10)
                .Matches("^([A-Z|a-z]{5,10})$");
        }
    }
}
