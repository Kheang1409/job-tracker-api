using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<string>;