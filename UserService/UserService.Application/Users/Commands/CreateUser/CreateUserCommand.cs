using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Email, string Password) : IRequest<string>;