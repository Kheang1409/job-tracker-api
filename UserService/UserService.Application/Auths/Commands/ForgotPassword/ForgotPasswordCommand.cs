using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<bool>;