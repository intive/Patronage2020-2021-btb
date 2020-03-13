using FluentValidation;

namespace BTB.Application.UserProfile.Commands.CreateUserProfileCommand
{
    internal class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
    {
        public CreateUserProfileCommandValidator()
        {
            RuleFor(command => command.Username)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(16);

            RuleFor(command => command.ProfileBio)
                .MaximumLength(256);

            RuleFor(command => command.FavouriteTradingPair)
                .MaximumLength(10)
                .Matches("^([A-Z|a-z]{5,10})$");
        }
    }
}