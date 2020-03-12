using FluentValidation;

namespace BTB.Application.UserProfile.Commands.UpdateUserProfileCommand
{
    internal class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
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