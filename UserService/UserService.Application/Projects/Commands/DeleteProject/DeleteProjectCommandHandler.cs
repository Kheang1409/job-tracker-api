using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;
    public DeleteProjectCommandHandler(
        IProjectRepository projectRepository
    )
    {
        _projectRepository = projectRepository;
    }
    
    public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        return await _projectRepository.DeleteAsync(command.UserId, command.ProjectId);
    }
}