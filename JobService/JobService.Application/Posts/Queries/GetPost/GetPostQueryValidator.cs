using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetPost;

public class GetPostQueryValidator : AbstractValidator<GetPostQuery>
{
    public GetPostQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage("Post Id is required.")
            .Must(BeAValidObjectId).WithMessage("Post Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}