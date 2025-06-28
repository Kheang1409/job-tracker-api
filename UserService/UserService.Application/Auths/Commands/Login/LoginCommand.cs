using MediatR;

namespace JobTracker.UserService.Application.Auths.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<string>;