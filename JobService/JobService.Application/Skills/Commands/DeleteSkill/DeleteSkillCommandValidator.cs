using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.Skills.Commands.DeleteSkill;

public class DeleteSkillCommandValidator : AbstractValidator<DeleteSkillCommand>
{
    public DeleteSkillCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required.")
            .Must(BeAValidObjectId).WithMessage("User id must be a valid MongoDB ObjectId.");

        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job Post Id is required.")
            .Must(BeAValidObjectId).WithMessage("Job Post id must be a valid MongoDB ObjectId.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("Skill Id is required.")
            .Must(BeAValidObjectId).WithMessage("Skill id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}
