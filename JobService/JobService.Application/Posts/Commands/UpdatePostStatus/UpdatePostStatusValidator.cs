using FluentValidation;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdatePostStatus;

public class UpdatePostStatusCommandValidator : AbstractValidator<UpdateStatusPostCommand>
{
    public UpdatePostStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Job Status is required.");
    }
}
