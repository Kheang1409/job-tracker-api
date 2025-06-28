using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.Skills.Queries.GetSkill;

public class GetSkillQueryValidator : AbstractValidator<GetSkillQuery>
{
    public GetSkillQueryValidator()
    {
        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job Post Id is required.")
            .Must(BeAValidObjectId).WithMessage("User Id must be a valid MongoDB ObjectId.");
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("Skill Id is required.")
            .Must(BeAValidObjectId).WithMessage("Skill Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}