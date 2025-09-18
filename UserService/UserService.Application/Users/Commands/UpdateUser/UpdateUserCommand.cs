using JobTracker.UserService.Application.Users.Commands.UpdateUser.Commons;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string FirstName,
    string LastName,
    string Bio,
    string Email,
    string ContactNumber,
    List<string> Skills,
    List<ExperienceCommand> Experiences,
    List<ProjectCommand> Projects,
    AddressCommand Address
) : IRequest<bool>;


