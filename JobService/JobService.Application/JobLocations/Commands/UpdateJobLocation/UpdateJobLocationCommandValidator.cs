using FluentValidation;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobLocation;

public class UpdateJobLocationCommandValidator : AbstractValidator<UpdateJobLocationCommand>
{
    public UpdateJobLocationCommandValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.");

        RuleFor(x => x.PostalCode)
            .GreaterThan(0).WithMessage("Postal code must be a positive number.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.County)
            .NotEmpty().WithMessage("County is required.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.");
    }
}
