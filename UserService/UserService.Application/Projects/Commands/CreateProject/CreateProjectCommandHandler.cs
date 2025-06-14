using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using MediatR;

namespace JobTracker.UserService.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectWithIdCommand, string>
{
    private readonly IProjectRepository _projectRepository;
    public CreateProjectCommandHandler(
        IProjectRepository projectRepository
    )
    {
        _projectRepository = projectRepository;
    }
    
    public async Task<string> Handle(CreateProjectWithIdCommand command, CancellationToken cancellationToken)
    {
        var project = Project.Create(
            command.Name,
            command.About,
            command.StartDate,
            command.EndDate
        );
        var projectId = await _projectRepository.AddAsync(command.UserId, project);
        return projectId;
    }
}