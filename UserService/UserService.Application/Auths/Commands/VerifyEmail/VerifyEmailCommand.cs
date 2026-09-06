using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.VerifyEmail;

public record VerifyEmailCommand(string Email, string Token) : IRequest<bool>;
