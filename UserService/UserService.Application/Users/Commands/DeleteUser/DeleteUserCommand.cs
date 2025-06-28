using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest<bool>;