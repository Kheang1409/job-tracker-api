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
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required.")
            .Matches(@"^\+\d{1,3}\d{7,15}$")
            .WithMessage("Phone Number must start with a '+' followed by country code and 7 to 15 digits.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street must not exceed 100 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

        RuleFor(x => x.PostalCode)
            .InclusiveBetween(1000, 999999).WithMessage("PostalCode must be between 4 to 6 digits.");

    }
}
