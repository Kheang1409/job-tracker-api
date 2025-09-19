using FluentValidation;

namespace JobTracker.JobService.Application.JobLocations.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(100).WithMessage("Job title must be at most 100 characters.");

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must be at most 100 characters.");
        
        RuleFor(x => x.WorkMode.ToString())
            .NotEmpty().WithMessage("Work mode is required.")
            .MaximumLength(100).WithMessage("Work mode must be at most 100 characters.");

        RuleFor(x => x.EmploymentType.ToString())
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

        RuleFor(x => x.Skills)
            .NotNull().WithMessage("Required skills must be provided.")
            .Must(skills => skills.Count > 0).WithMessage("At least one skill is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
}
