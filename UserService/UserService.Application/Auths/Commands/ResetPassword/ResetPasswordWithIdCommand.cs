using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.ResetPassword;

public record ResetPasswordWithIdCommand(
    string userId,
    string OTP,
    string Password
) : IRequest<bool>;