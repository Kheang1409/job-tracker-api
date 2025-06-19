using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.Candidates.Rejected.Commands;

public class RejectedCommandValidator : AbstractValidator<RejectedCommand>
{
    public RejectedCommandValidator()
    {
        RuleFor(x => x.CandidateId)
             .NotEmpty().WithMessage("Candidate Id is required.")
             .Must(BeAValidObjectId).WithMessage("Candidate id must be a valid MongoDB ObjectId.");

        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job post Id is required.")
            .Must(BeAValidObjectId).WithMessage("Job post must be a valid MongoDB ObjectId.");
    }
    
    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}
