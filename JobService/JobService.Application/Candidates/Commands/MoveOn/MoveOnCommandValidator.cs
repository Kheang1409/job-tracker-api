using FluentValidation;

namespace JobTracker.JobService.Application.Candidates.MoveOn.Commands;

public class MoveOnCommandValidator : AbstractValidator<MoveOnCommand>
{
    public MoveOnCommandValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Stage name is required.");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("AppointmentDate Date is required.");

    }
}
