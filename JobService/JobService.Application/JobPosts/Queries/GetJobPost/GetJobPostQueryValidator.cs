using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.JobLocations.Queries.GetJobPost;

public class GetJobPostQueryValidator : AbstractValidator<GetJobPostQuery>
{
    public GetJobPostQueryValidator()
    {
        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job Post Id is required.")
            .Must(BeAValidObjectId).WithMessage("Job Post Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}