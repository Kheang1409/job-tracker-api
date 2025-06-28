using JobTracker.UserService.Application.Repositories;
using MediatR;

namespace JobTracker.UserService.Application.Skills.Commands.DeleteSkill;
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
        return await _skillRepository.DeleteAsync(command.UserId, command.SkillId);
    }
}