using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.UserService.Application.Skills.Queries.GetSkills;

public class GetSkillsQueryValidator : AbstractValidator<GetSkillsQuery>
{
    public GetSkillsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required.")
            .Must(BeAValidObjectId).WithMessage("User Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}