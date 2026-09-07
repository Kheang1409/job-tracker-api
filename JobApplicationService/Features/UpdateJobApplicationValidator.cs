using FluentValidation;

namespace JobTracker.JobApplicationService.Application.JobApplications;

public sealed class UpdateJobApplicationValidator : AbstractValidator<UpdateJobApplicationCommand>
{
    public UpdateJobApplicationValidator()
    {
        RuleFor(request => request.UserId).NotEmpty();
        RuleFor(request => request.Id).NotEmpty();
        RuleFor(request => request.Company).NotEmpty().MaximumLength(200);
        RuleFor(request => request.Role).NotEmpty().MaximumLength(200);
        RuleFor(request => request.Status).Must(JobApplicationStatus.IsValid)
            .WithMessage("Status must be Saved, Applied, Interview, Offer, or Rejected.");
        RuleFor(request => request.Url).MaximumLength(2048)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("URL must be an absolute URL.");
        RuleFor(request => request.Notes).MaximumLength(10_000);
    }
}
