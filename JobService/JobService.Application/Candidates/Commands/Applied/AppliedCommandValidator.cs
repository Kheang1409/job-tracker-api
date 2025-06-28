using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.JobService.Application.Candidates.Applied.Commands;

public class AppliedCommandValidator : AbstractValidator<AppliedCommand>
{
    public AppliedCommandValidator()
    {
        RuleFor(x => x.CandidateId)
             .NotEmpty().WithMessage("Candidate Id is required.")
             .Must(BeAValidObjectId).WithMessage("Candidate id must be a valid MongoDB ObjectId.");

        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job post Id is required.")
            .Must(BeAValidObjectId).WithMessage("Job post must be a valid MongoDB ObjectId.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Job post Id is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Job post Id is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        
    }
    
    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}
