using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string FirstName,
    string LastName,
    string Bio,
    string Email,
    string ContactNumber,
    List<string> Skills,
    List<Experience> Experiences,
    List<Project> Projects,
    Address Address
) : IRequest<bool>;


