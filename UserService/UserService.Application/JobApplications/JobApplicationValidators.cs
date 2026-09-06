using FluentValidation;

namespace JobTracker.UserService.Application.JobApplications;

public sealed class CreateJobApplicationValidator : AbstractValidator<CreateJobApplicationCommand>
{
    public CreateJobApplicationValidator()
    {
        RuleFor(request => request.UserId).NotEmpty();
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

internal static class JobApplicationStatus
{
    private static readonly string[] Values = ["Saved", "Applied", "Interview", "Offer", "Rejected"];
    public static bool IsValid(string status) => Values.Contains(status, StringComparer.OrdinalIgnoreCase);
}
