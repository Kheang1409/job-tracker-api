using FluentValidation;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateJobPost;

public class UpdateJobPostCommandValidator : AbstractValidator<UpdateJobPostCommand>
{
    public UpdateJobPostCommandValidator()
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
        
        RuleFor(x => x.JobDescription)
            .NotEmpty().WithMessage("Job description is required.");

    }
}
