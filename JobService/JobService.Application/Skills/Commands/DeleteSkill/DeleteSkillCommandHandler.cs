using JobTracker.JobService.Application.CustomExceptions;
using JobTracker.JobService.Application.Repositories;
using MediatR;

namespace JobTracker.JobService.Application.Skills.Commands.DeleteSkill;
public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, bool>
{
    private readonly ISkillRepository _skillRepository;
    public DeleteSkillCommandHandler(
        ISkillRepository skillRepository
    )
    {
        _skillRepository = skillRepository;
    }
    
    public async Task<bool> Handle(DeleteSkillCommand command, CancellationToken cancellationToken)
    {
        return await _skillRepository.DeleteAsync(command.AuthorId, command.JobPostId, command.SkillId);
    }
}