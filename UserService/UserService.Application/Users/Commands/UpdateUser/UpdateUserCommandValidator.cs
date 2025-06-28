using FluentValidation;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Firstname is required.")
            .Length(2).WithMessage("Firstname must be valid.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Lastname is required.")
            .Length(2).WithMessage($"Lastname must be valid.");
        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be valid.");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country Code is required.")
            .Matches(@"^\+?[1-9]\d{0,3}$").WithMessage("Country Code must be a valid international code (e.g., +1, +44, 91).");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required.")
            .Matches(@"^[0-9]{7,15}$").WithMessage("Phone Number must be between 7 and 15 digits.");
    }
}
