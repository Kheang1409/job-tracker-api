using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.UserService.Application.Projects.Queries.GetProject;

public class GetProjectQueryValidator : AbstractValidator<GetProjectQuery>
{
    public GetProjectQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required.")
            .Must(BeAValidObjectId).WithMessage("User Id must be a valid MongoDB ObjectId.");
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project Id is required.")
            .Must(BeAValidObjectId).WithMessage("Project Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}