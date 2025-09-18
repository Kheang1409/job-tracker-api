using MediatR;
using JobTracker.UserService.Application.Users.Commands.UpdateUser.Commons;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public record UpdateUserWithIdCommand(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string ContactNumber,
    string Bio,
    List<string> Skills,
    List<ExperienceCommand> Experiences,
    List<ProjectCommand> Projects,
    AddressCommand Address
) : IRequest<bool>;