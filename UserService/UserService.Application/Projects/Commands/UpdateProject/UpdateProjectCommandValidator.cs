using FluentValidation;

namespace JobTracker.UserService.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.About)
            .MaximumLength(100).WithMessage("About must not exceed 100 characters.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("StartDate cannot be in the future.");

    }
}
