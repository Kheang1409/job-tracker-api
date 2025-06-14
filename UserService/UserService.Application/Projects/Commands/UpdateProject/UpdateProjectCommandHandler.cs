using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Projects.Commands.UpdateProject;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateProjectWithIdCommand, bool>
{
    private readonly IProjectRepository _projectRepository;
    public UpdateAddressCommandHandler(
        IProjectRepository projectRepository
    )
    {
        _projectRepository = projectRepository;
    }
    
    public async Task<bool> Handle(UpdateProjectWithIdCommand command, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(command.UserId, command.ProjectId);
        project.Update(command.Name,
            command.About,
            command.StartDate,
            command.EndDate);
        return await _projectRepository.UpdateAsync(command.UserId, project);
    }
}