using FluentValidation;

namespace JobTracker.JobService.Application.Candidates.Queries.GetCandidates;

public class GetCandidatesQueryValidator : AbstractValidator<GetCandidatesQuery>
{
    public GetCandidatesQueryValidator()
    {
        RuleFor(x => x.Limit)
            .NotEmpty().WithMessage("Limit is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Limit must greater than or equal 1");
        RuleFor(x => x.PageNumber)
            .NotEmpty().WithMessage("Page number is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Page number must greater than or equal 1");
    }
}