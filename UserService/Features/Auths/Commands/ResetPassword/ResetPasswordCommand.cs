using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.ResetPassword;

public record ResetPasswordCommand(string Email, string OTP, string Password) : IRequest<bool>;