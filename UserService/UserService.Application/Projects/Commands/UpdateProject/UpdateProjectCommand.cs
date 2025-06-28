using MediatR;

namespace JobTracker.UserService.Application.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(
    string Name,
    string About,
    DateTime StartDate,
    DateTime EndDate) : IRequest<bool>;