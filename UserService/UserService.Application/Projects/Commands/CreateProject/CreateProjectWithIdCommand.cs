using MediatR;

namespace JobTracker.UserService.Application.Projects.Commands.CreateProject;

public record CreateProjectWithIdCommand(
    string UserId,
    string Name,
    string About,
    DateTime StartDate,
    DateTime EndDate) : IRequest<string>;