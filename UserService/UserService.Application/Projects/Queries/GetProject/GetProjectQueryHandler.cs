using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Projects.Queries.GetProject;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, Project>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectQueryHandler(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public async Task<Project> Handle(GetProjectQuery command, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(command.UserId, command.ProjectId);
        if(project is null)
            throw new InvalidOperationException($"The project is not exits.");
        return project;
    }
}