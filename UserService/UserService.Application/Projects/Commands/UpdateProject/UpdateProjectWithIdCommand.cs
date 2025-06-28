using MediatR;

namespace JobTracker.UserService.Application.Projects.Commands.UpdateProject;

public record UpdateProjectWithIdCommand(
    string UserId,
    string ProjectId,
    string Name,
    string About,
    DateTime StartDate,
    DateTime EndDate) : IRequest<bool>;