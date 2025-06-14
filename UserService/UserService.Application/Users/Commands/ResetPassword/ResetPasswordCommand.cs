using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.ResetPassword;

public record ResetPasswordCommand(string OTP, string Password) : IRequest<bool>;