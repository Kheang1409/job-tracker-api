using FluentValidation;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(100).WithMessage("Job title must be at most 100 characters.");

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name must be at most 100 characters.");

        RuleFor(x => x.NumberOfOpenings)
            .GreaterThan(0).WithMessage("There must be at least one opening.");

        RuleFor(x => x.MinExperience)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum experience cannot be negative.");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Job description is required.");

    }
}
