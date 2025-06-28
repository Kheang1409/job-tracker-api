using FluentValidation;

namespace JobTracker.UserService.Application.Skills.Commands.UpdateSkill;

public class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
{
    public UpdateSkillCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}
