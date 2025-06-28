using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<Project>>
{
    
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public async Task<IEnumerable<Project>> Handle(GetProjectsQuery command, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(command.UserId);
        return projects;
    }
}