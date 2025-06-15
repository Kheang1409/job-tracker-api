using FluentValidation;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdateStatusJobPost;

public class UpdateStatusJobPostCommandValidator : AbstractValidator<UpdateStatusJobPostCommand>
{
    public UpdateStatusJobPostCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Job Status is required.");
    }
}
