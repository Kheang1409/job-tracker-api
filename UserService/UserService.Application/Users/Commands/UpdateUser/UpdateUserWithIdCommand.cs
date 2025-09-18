using MediatR;
using JobTracker.UserService.Domain.Entities;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public record UpdateUserWithIdCommand(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string ContactNumber,
    string Bio,
    List<string> Skills,
    List<Experience> Experiences,
    List<Project> Projects,
    Address Address
) : IRequest<bool>;