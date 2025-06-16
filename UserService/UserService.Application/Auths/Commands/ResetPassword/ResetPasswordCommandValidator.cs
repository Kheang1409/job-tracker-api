using FluentValidation;

namespace JobTracker.UserService.Application.Auths.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.OTP)
            .NotEmpty().WithMessage("OPT is required.")
            .MinimumLength(6).WithMessage("OTP must be 6 characters.")
            .MaximumLength(6).WithMessage("OTP must be 6 characters.")
            .Matches(@"[0-9]").WithMessage("OPT must be numbers.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.");
    }
}