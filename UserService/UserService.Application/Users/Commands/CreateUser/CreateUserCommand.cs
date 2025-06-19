using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Firstname, string Lastname, string Email, string Password) : IRequest<string>;