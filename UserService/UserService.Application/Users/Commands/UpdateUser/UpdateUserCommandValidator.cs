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
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be valid.");
        RuleFor(x => x.ContactNumber)
            .NotEmpty().WithMessage("Phone Number is required.")
            .Matches(@"^\+\d{1,3}\d{7,15}$")
            .WithMessage("Phone Number must start with a '+' followed by country code and 7 to 15 digits.");
    }
}
