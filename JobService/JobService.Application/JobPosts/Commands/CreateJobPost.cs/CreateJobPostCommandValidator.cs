using FluentValidation;

namespace JobTracker.JobService.Application.JobLocations.Commands.CreateJobPost;

public class CreateJobPostCommandValidator : AbstractValidator<CreateJobPostCommand>
{
    public CreateJobPostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(100).WithMessage("Job title must be at most 100 characters.");

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must be at most 100 characters.");
        
        RuleFor(x => x.WorkMode)
            .NotEmpty().WithMessage("Work mode is required.")
            .MaximumLength(100).WithMessage("Work mode must be at most 100 characters.");

        RuleFor(x => x.EmploymentType)
            .NotEmpty().WithMessage("Employment type is required.")
            .MaximumLength(100).WithMessage("Employment type must be at most 100 characters.");

        RuleFor(x => x.NumberOfOpenings)
            .GreaterThan(0).WithMessage("There must be at least one opening.");

        RuleFor(x => x.MinExperience)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum experience cannot be negative.");

        RuleFor(x => x.MinSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum salary cannot be negative.");

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(x => x.MinSalary)
            .WithMessage("Maximum salary must be greater than or equal to minimum salary.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.");

        RuleFor(x => x.RequiredSkills)
            .NotNull().WithMessage("Required skills must be provided.")
            .Must(skills => skills.Count > 0).WithMessage("At least one skill is required.");

        RuleFor(x => x.JobDescription)
            .NotEmpty().WithMessage("Job description is required.");

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

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.");
    }
}
