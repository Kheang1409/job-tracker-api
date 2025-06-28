using MediatR;

namespace JobTracker.UserService.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(string UserId, string ProjectId) : IRequest<bool>;