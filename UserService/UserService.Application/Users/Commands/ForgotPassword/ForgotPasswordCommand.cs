using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<bool>;