using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.JobLocations.Commands.DeleteJobPost;

public class DeleteJobPostCommandValidator : AbstractValidator<DeleteJobPostCommand>
{
    public DeleteJobPostCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Author Id is required.")
            .Must(BeAValidObjectId).WithMessage("Author id must be a valid MongoDB ObjectId.");

        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job post Id is required.")
            .Must(BeAValidObjectId).WithMessage("Job post must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}
