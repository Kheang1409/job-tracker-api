using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.Skills.Queries.GetSkills;

public class GetSkillsQueryValidator : AbstractValidator<GetSkillsQuery>
{
    public GetSkillsQueryValidator()
    {
        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job PostId Id is required.")
            .Must(BeAValidObjectId).WithMessage("Job PostId Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}