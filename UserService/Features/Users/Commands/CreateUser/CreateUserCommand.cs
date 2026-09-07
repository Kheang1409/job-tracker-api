using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Username, string Firstname, string Lastname, string Email, string Password) : IRequest<string>;