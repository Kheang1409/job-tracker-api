using FluentValidation;

namespace JobTracker.UserService.Application.Addresses.Commands.CreateAddress;


public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x.Address1)
            .NotEmpty().WithMessage("Address1 is required.")
            .MaximumLength(100).WithMessage("Address1 must not exceed 100 characters.");

        RuleFor(x => x.PostalCode)
            .InclusiveBetween(1000, 999999).WithMessage("PostalCode must be between 4 to 6 digits.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

        RuleFor(x => x.County)
            .NotEmpty().WithMessage("County is required.")
            .MaximumLength(50).WithMessage("County must not exceed 50 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

        When(x => CountriesRequiringState.Contains(x.Country), () =>
        {
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required for the selected country.")
                .MaximumLength(50).WithMessage("State must not exceed 50 characters.");
        });

        When(x => !CountriesRequiringState.Contains(x.Country), () =>
        {
            RuleFor(x => x.State)
                .Must(string.IsNullOrWhiteSpace)
                .WithMessage("State should be empty for the selected country.");
        });
    }

    private static readonly string[] CountriesRequiringState = new[]
    {
        "United States", "Canada", "Australia", "India", "Brazil", "Mexico"
    };
}
